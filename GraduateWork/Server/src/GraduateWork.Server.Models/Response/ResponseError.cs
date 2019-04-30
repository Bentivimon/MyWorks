using System.Globalization;
using GraduateWork.Server.Models.CustomExceptions;
using Newtonsoft.Json;

namespace GraduateWork.Server.Models.Response
{
    public class ResponseError
    {
        [JsonIgnore]
        public string Code { get; set; }

        public string Message { get; set; }

        public object Errors { get; set; }

        public ResponseError()
        { }

        public ResponseError(BaseException ex)
        {
            Code = ex.ErrorCode.ToString(CultureInfo.InvariantCulture);
            Message = ex.Message;
            Errors = ex.Errors;
        }
    }
}
