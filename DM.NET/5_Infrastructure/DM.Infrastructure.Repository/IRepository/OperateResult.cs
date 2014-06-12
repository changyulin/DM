
namespace DM.Infrastructure.Repository
{
    /// <summary>
    /// add, delete, update操作结果
    /// </summary>
    public class OperateResult
    {
        /// <summary>
        /// 0:false, 1:true, 2: other ... 
        /// </summary>
        private int result;
        public int Result
        {
            get { return result; }
            set { result = value; }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public OperateResult() : this(0, null) { }

        public OperateResult(int result) : this(result, null) { }

        public OperateResult(int result, string message)
        {
            this.result = result;
            this.message = message;
        }
    }
}
