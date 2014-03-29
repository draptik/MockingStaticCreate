using System;

namespace MockingStaticCreate.App
{
    public class SomeService
    {
        /// <summary>
        ///     Dummy method comparing read-only property
        /// </summary>
        public bool HaveSameId(User user1, User user2)
        {
            if (user1.Id == null) throw new ArgumentNullException("user1");
            if (user2.Id == null) throw new ArgumentNullException("user2");
            //if (!user1.Id.HasValue || !user2.Id.HasValue) return false;

            return user1.Id.Value == user2.Id.Value;
        }
    }
}