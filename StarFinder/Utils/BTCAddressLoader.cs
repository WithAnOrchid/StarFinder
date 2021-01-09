using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;

namespace StarFinder.Utils
{
    public static class BTCAddressLoader
    {
        public static readonly HashSet<string> AddressBook;

        static BTCAddressLoader()
        {
            AddressBook = new HashSet<string>();
        }

        public static void LoadAddresses(IProgress<int> progress = null)
        {
            int counter = 0;
            using (StreamReader reader = new StreamReader(Config.AddressCSVPath))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    AddressBook.Add(csv.GetField("address"));
                    counter++;

                    if (counter % 1000000 == 0)
                    {
                        progress?.Report(counter);
                    }
                }
                progress?.Report(counter);
            }
        }

        public static async Task<int> LoadAddressesAsync(IMyLogger logger)
        {
            Progress<int> progress = new Progress<int>(count =>
            {
               logger.Info($"Loaded {count} addresses");
            });
            await Task.Run(() => LoadAddresses(progress));
            return AddressBook.Count;
        }
    }

}