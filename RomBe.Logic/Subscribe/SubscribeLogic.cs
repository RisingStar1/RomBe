using RomBe.Entities;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Logic.Subscribe
{
    public class SubscribeLogic
    {
        public async Task<SubscribeResponse> Subscribe(string emailAddress)
        {
            string _subscribeSucceedCode = "I4";
            string _subscribeFaildCode = "I5";
            string _faildToInsertToDbCode = "I6";
            SystemMessage _message;
            bool _isSent = false;
            bool _isCreated = new SubscribeDAL().Create(emailAddress);
            if (_isCreated)
            {
                _isSent = await new EmailLogic().SendSubscribeEmailAsync(emailAddress);
                if (_isSent)
                {
                    _message = await new SystemMessageDAL().GetSystemMessageAsync(_subscribeSucceedCode);
                }
                else
                {
                    _message = await new SystemMessageDAL().GetSystemMessageAsync(_subscribeFaildCode);
                }

            }
            else
            {
                _message = await new SystemMessageDAL().GetSystemMessageAsync(_faildToInsertToDbCode);
            }

            return new SubscribeResponse()
            {
                IsSent = _isSent,
                Message = _message.MessageContent
            };
        }
    }
}
