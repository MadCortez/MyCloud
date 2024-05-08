using Microsoft.AspNetCore.Mvc;

namespace MyCloud.Implementations
{
    public class FileCleanupStreamResult : FileStreamResult
    {
        private readonly Action _cleanupAction;

        public FileCleanupStreamResult(Stream fileStream, string contentType, Action cleanupAction)
            : base(fileStream, contentType)
        {
            _cleanupAction = cleanupAction;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await base.ExecuteResultAsync(context);
            _cleanupAction?.Invoke();
        }
    }
}
