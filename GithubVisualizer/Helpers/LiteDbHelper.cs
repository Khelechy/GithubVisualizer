using GithubVisualizer.Models.DataModels;
using LiteDB;

namespace GithubVisualizer.Helpers
{
	public static class LiteDbHelper
	{
        public static LiteDatabase db = new LiteDatabase("visualiserdb.db");
        static ILiteCollection<Repository> _db = db.GetCollection<Repository>(BsonAutoId.Int32);

        public static IEnumerable<Repository> GetAll()
        {
            return _db.FindAll();
        }

        public static Repository GetById(int id)
        {
            return _db.FindById(id);
        }

        public static void Insert(Repository repo)
        {
            _db.Insert(repo);
        }

        public static bool Update(Repository repo)
        {
            return _db.Update(repo);
        }

        public static bool Delete(int id)
        {
            return _db.Delete(id);
        }
    }
}
