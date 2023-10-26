using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TechChallengeAuth.Controllers;

[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    public ValuesController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
    {
    }

    // GET api/values
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}