using Microsoft.EntityFrameworkCore;
using System.Linq;
using tgropa.Data;
namespace tgropa.Core
{
    public class ItemEF : IDataHelper<Item>
    {
        private readonly DataBaseContext db;

        public ItemEF()
        {
            db = new DataBaseContext();
        }

        public int Add(Item entity)
        {
            try
            {
                db.Items.Add(entity);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        public int Delete(int Id)
        {
            try
            {
                var item = Find(Id);
                if (item != null)
                {
                    db.Items.Remove(item);
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        public Item Find(int Id)
        {
            try
            {
                return db.Items.FirstOrDefault(x => x.Id == Id) ?? new Item();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Item();
            }
        }

        public List<Item> GetAllData()
        {
            try
            {
                return db.Items.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Item>();
            }
        }

        public List<Item> Search(string searchItem)
        {
            try
            {
                return db.Items.Where(x => x.Name.Contains(searchItem)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Item>();
            }
        }

        public int Update(Item entity)
        {
            try
            {
                db.Items.Update(entity);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
    }
}
