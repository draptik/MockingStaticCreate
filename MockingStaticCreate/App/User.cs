namespace MockingStaticCreate.App
{
    public class User
    {
        private User()
        {
        }

        /// <summary>
        ///     Created by Database...
        /// </summary>
        public long? Id { get; protected set; }

        public string Name { get; protected set; }

        public static User Create(string name)
        {
            var user = new User {Name = name};
            return user;
        }
    }
}