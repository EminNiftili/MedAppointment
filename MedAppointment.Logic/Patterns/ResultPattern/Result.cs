namespace MedAppointment.Logics.Patterns.ResultPattern
{
    public class Result
    {
        public static Result Create()
        {
            return new Result();
        }
        public static Result Create(IEnumerable<Message> messages)
        {
            return new Result(messages);
        }


        protected Result()
        {
            Messages = new List<Message>();
            HttpStatus = HttpStatusCode.OK;
        }
        protected Result(IEnumerable<Message> messages)
        {
            Messages = new List<Message>(messages);
            HttpStatus = HttpStatusCode.OK;
        }
        public HttpStatusCode HttpStatus { get; protected set; }
        public List<Message> Messages { get; set; }

        public bool IsSuccess()
        {
            return (int)HttpStatus >= 200 && (int)HttpStatus < 300;
        }
        public Result MergeMessages(Result result)
        {
            this.Messages.AddRange(result.Messages);
            return this;
        }
        public Result MergeStatusCode(Result result)
        {
            SetStatusCode(result.HttpStatus);
            return this;
        }
        public Result MergeResult(Result result)
        {
            MergeMessages(result);
            MergeStatusCode(result);
            return this;
        }
        /// <summary>
        /// Gets all error from Validation Result, Then setted BadRequest status code.
        /// </summary>
        /// <param name="validationResult">Validation result for getting errors</param>
        /// <returns></returns>
        public bool SetFluentValidationAndBadRequest(ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return true;
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    this.AddMessage(new Message(error.ErrorCode, error.ErrorMessage));
                }
                this.SetStatusCode(HttpStatusCode.BadRequest);
                return false;
            }
        }
        public void AddMessage(Message message)
        {
            Messages.Add(message);
        }
        public void AddMessage(Message message, HttpStatusCode httpStatus)
        {
            Messages.Add(message);
            SetStatusCode(httpStatus);
        }
        public void AddMessage(string messageCode, string message, Exception exception = null)
        {
            Messages.Add(new Message(messageCode, message, exception));
        }
        public void AddMessage(string messageCode, string message, HttpStatusCode httpStatus, Exception exception = null)
        {
            Messages.Add(new Message(messageCode, message, exception));
            SetStatusCode(httpStatus);
        }
        public void Success(HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            SetStatusCode(httpStatusCode);
        }

        public void SetStatusCode(HttpStatusCode newStatusCode)
        {
            // Əgər artıq error status qoyulubsa (>= 400),
            // və yeni gələn successdirsə (< 400) → dəyişmə.
            if ((int)HttpStatus >= 400 && (int)newStatusCode < 400)
                return;

            // Əks halda, ən son gələn statusla update et.
            HttpStatus = newStatusCode;
        }
    }
}
