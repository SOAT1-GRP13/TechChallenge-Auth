﻿using Application.Autenticacao.Boundaries.Cliente;
using Application.Autenticacao.Boundaries.LogIn;
using Application.Autenticacao.Dto;
using Application.Autenticacao.Dto.Cliente;

namespace Application.Autenticacao.UseCases
{
    public interface IAutenticacaoUseCase : IDisposable
    {
        Task<LogInUsuarioOutput> AutenticaUsuario(LoginUsuarioDto input);
        Task<AutenticaClienteOutput> AutenticaCliente(IdentificaDto input);
        Task<AutenticaClienteOutput> AutenticaClientePorNome(string nome);
        string EncryptPassword(string dataToEncrypt);
    }
}
