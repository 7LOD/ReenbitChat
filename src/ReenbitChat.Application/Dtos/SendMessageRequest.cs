using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitChat.Application.Dtos
{
    public record SendMessageRequest(string Room, string UserName, string Text);
}
