using System.Text.Json.Serialization;

namespace Backend.Api.Models.Api.Response
{
    /// <summary>
    /// 基本的API回傳格式
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 成功或失敗 (前端用來判斷流程是否繼續)
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; } = true;

        /// <summary>
        /// 訊息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = "Success";
    }
}
