using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cosmodb01
{
    public class LogHandler : RequestHandler
    {
        public override async Task<ResponseMessage> SendAsync(RequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[{request.Method.Method}]\t{request.RequestUri}");

            ResponseMessage response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine($"[{Convert.ToInt32(response.StatusCode)}]\t{response.StatusCode}");

            return response;
        }
    }
}
