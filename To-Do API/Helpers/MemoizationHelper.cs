namespace To_Do_API.Helpers
{
    public static class MemoizationHelper
    {

        private static readonly Dictionary<string, double> _completionCache = new();

        public static double CalculateCompletionPercentage(int completedTasks, int totalTasks)
        {
            if (totalTasks == 0) return 0;
            var key = $"{completedTasks}-{totalTasks}";
            if (_completionCache.TryGetValue(key, out var cachedValue))
            {
                return cachedValue;
            }
            var percentage = (double)completedTasks / totalTasks * 100;
            _completionCache[key] = percentage;
            return percentage;
        }

    }
}
