﻿using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Dto;
using Application.Autenticacao.Dto.Cliente;
using Domain.Autenticacao;
using Domain.Autenticacao.Enums;
using Domain.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Autenticacao.UseCases
{
    public class AutenticacaoUseCase : IAutenticacaoUseCase
    {
        private readonly IAutenticacaoRepository _autenticacaoRepository;
        private readonly IUsuarioLogadoRepository _UsuarioLogadoRepository;
        private readonly Secrets _settings;

        public AutenticacaoUseCase(IAutenticacaoRepository autenticacaoRepository,
         IUsuarioLogadoRepository usuarioLogadoRepository,
          IOptions<Secrets> options)
        {
            _autenticacaoRepository = autenticacaoRepository;
            _UsuarioLogadoRepository = usuarioLogadoRepository;
            _settings = options.Value;
        }

        public async Task<LogInUsuarioOutput> AutenticaUsuario(LoginUsuarioDto input)
        {
            var usuario = new AcessoUsuario(input.NomeUsuario, input.Senha);

            var autenticado = await _autenticacaoRepository.AutenticaUsuario(usuario);

            if (!string.IsNullOrEmpty(autenticado.NomeUsuario))
            {
                var token = GenerateToken(autenticado.NomeUsuario, autenticado.Role.ToString(), autenticado.Id, string.Empty);
                await _UsuarioLogadoRepository.AddUsuarioLogado(token);
                return new LogInUsuarioOutput(input.NomeUsuario, token);
            }

            return new LogInUsuarioOutput();
        }

        public async Task<AutenticaClienteOutput> AutenticaCliente(IdentificaDto input)
        {
            var usuario = new AcessoCliente(input.CPF, input.Senha);

            var autenticado = await _autenticacaoRepository.AutenticaCliente(usuario);

            if (string.IsNullOrEmpty(autenticado.CPF))
                return new AutenticaClienteOutput();

            var token = GenerateToken(autenticado.Nome, Roles.Cliente.ToString(), autenticado.Id, autenticado.Email);

            await _UsuarioLogadoRepository.AddUsuarioLogado(token);
            return new AutenticaClienteOutput(autenticado.Nome, token);
        }

        public async Task<AutenticaClienteOutput> AutenticaClientePorNome(string nome)
        {
            var guid = Guid.NewGuid();

            var token = GenerateToken(nome, Roles.ClienteSemCpf.ToString(), guid, string.Empty);

            await _UsuarioLogadoRepository.AddUsuarioLogado(token);

            return new AutenticaClienteOutput(nome, token);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _autenticacaoRepository.Dispose();
        }


        private string GenerateToken(string name, string role, Guid idUsuario, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _settings.ClientSecret;
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string EncryptPassword(string dataToEncrypt)
        {
            string encryptedData;
            var bytes = Encoding.UTF8.GetBytes($"{_settings.PreSalt}{dataToEncrypt}{_settings.PosSalt}");
            var hash = SHA512.HashData(bytes);
            encryptedData = GetStringFromHash(hash);

            return encryptedData;
        }

        private static string GetStringFromHash(byte[] hash)
        {
            var result = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}
