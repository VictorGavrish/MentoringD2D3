namespace Sources
{
    using System;

    public class SourceResult
    {
        public SourceResult(int id, int?[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.Id = id;
            this.Values = values;
        }

        public int Id { get; private set; }

        public int?[] Values { get; private set; }
    }
}