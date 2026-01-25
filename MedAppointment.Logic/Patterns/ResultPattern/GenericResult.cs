namespace MedAppointment.Logics.Patterns.ResultPattern
{
    public class Result<TModel> : Result
    {
        public static new Result<TModel> Create()
        {
            return new Result<TModel>();
        }
        public static Result<TModel> Create(TModel model, IEnumerable<Message> messages = null)
        {
            messages ??= new List<Message>();
            return new Result<TModel>(model, messages);
        }
        public static new Result<TModel> Create(IEnumerable<Message> messages)
        {
            return new Result<TModel>(default, messages);
        }


        protected Result() : base()
        {

        }
        protected Result(TModel model) : base()
        {
            Model = model;
        }
        protected Result(TModel? model, IEnumerable<Message> messages) : base(messages)
        {
            Model = model;
        }
        public TModel? Model { get; set; }

        public Result<TModel> MergeData<TObject>(Result<TObject> result) where TObject : TModel
        {
            this.Model = result.Model;
            return this;
        }
        public Result<TModel> MergeResult<TObject>(Result<TObject> result) where TObject : TModel
        {
            MergeMessages(result);
            MergeStatusCode(result);
            MergeData(result);
            return this;
        }

        public void Success(TModel model, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            this.Model = model;
            SetStatusCode(httpStatusCode);
        }
    }
}
