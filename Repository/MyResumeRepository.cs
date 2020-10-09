using System.Threading.Tasks;
using AgentyTask;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AgentyTask.Repository
{
    public class MyResumeRepository
    {

        static MyResumeRepository()
        {
            BsonClassMap.RegisterClassMap<MyResume>(_u =>
            {
                _u.AutoMap();
                _u.SetIdMember(_u.GetMemberMap(a => a.Id));
            });
        }

        private IMongoCollection<MyResume> _MyResume;
        public MyResumeRepository()
        {
            var client = new MongoClient("mongodb+srv://Shashwatp:Shashwat234@cluster0.axdli.mongodb.net/AgentyJob?retryWrites=true&w=majority");
            var database = client.GetDatabase("test");
            _MyResume = database.GetCollection<MyResume>("resume");
        }

        public async Task<MyResume> GetByEmail(string email)
        {
            var resume = await _MyResume.Find(x => x.Email == email).FirstOrDefaultAsync();
            if (resume == null)
                return null;
            return resume;
        }

        public async Task<MyResume> Create(MyResume user)
        {
            await _MyResume.InsertOneAsync(user);
            return user;
        }

        public async Task<MyResume> GetById(string id)
        {
            var user = await _MyResume.Find(x => x.Id == id).FirstOrDefaultAsync();
            return user;
        }
    }
}