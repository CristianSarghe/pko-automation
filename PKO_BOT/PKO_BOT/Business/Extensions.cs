namespace PKO_BOT.Business
{
    public static class Extensions
    {
        public static bool ContainsSameOrder(this byte[] container, byte[] array)
        {
            for(var index = 0; index < container.Length - array.Length && index >= 0; ++index)
            {
                bool isSuccess = true;

                for(var currentIterationIndex = index; currentIterationIndex < index + array.Length; ++currentIterationIndex)
                {
                    if(array[currentIterationIndex - index] != container[currentIterationIndex])
                    {
                        isSuccess = false;
                        break;
                    }
                }

                if(isSuccess)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
