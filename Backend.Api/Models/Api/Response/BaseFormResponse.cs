using System.Text.Json.Serialization;

namespace Backend.Api.Models.Api.Response
{
    public class BaseFormResponse
    {
        /// <summary>
        /// 成功或失敗
        /// </summary>
        [JsonPropertyName("result")]
        public bool Result { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = "Success";

        /// <summary>
        /// 欄位檢查錯誤訊息
        /// </summary>
        [JsonPropertyName("errors")]
        public Dictionary<string, string> Errors { get; set; } = new();
    }
}