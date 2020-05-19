using projekt.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace projekt.MainViewModel
{
    class ViewModel : ViewModelBase
    {
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                onPropertyChanged(nameof(FilePath));
            }
        }

        private int? _p;
        public int? P
        {
            get => _p;
            set
            {
                _p = value;
                onPropertyChanged(nameof(P));
            }
        }

        private int? _q;
        public int? Q
        {
            get => _q;
            set
            {
                _q = value;
                onPropertyChanged(nameof(Q));
            }
        }


        private string _filePath2;
        public string FilePath2
        {
            get => _filePath2;
            set
            {
                _filePath2 = value;
                onPropertyChanged(nameof(FilePath2));
            }
        }

        private int? _e;
        public int? E
        {
            get => _e;
            set
            {
                _e = value;
                onPropertyChanged(nameof(E));
            }
        }

        private int? _n;
        public int? N
        {
            get => _n;
            set
            {
                _n = value;
                onPropertyChanged(nameof(N));
            }
        }


        private ICommand _kodowanie = null;
        public ICommand Kodowanie
        {
            get
            {
                if(_kodowanie == null)
                {
                    _kodowanie = new RelayCommand(
                        arg => {
                            KodowanieRSA(FilePath, P, Q);
                        },
                        arg => true
                        );
                }
                return _kodowanie;
            }
        }

        private ICommand _dekodowanie = null;
        public ICommand Dekodowanie
        {
            get
            {
                if(_dekodowanie == null)
                {
                    _dekodowanie = new RelayCommand(
                        arg => {
                            DekodowanieRSA(FilePath2, E, N);
                        },
                        arg => true
                        );
                }

                return _dekodowanie;
            }
        }


        private void KodowanieRSA(string filePath, int? p, int? q)
        {
            int n = (int)(p * q);
            int phi = ((int)p - 1) * ((int)q - 1);
            int e = 3;
            while(NWD(e, phi) != 1)
            {
                e += 2;
            }
            int d = odwMod(e, phi);

            if(File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                string[] wynik = new string[lines.Length];

                StreamWriter streamWriter;
                streamWriter = File.AppendText(filePath.Substring(0, filePath.Length - filePath.Substring(filePath.LastIndexOf(@".")).Length)+"_zakodowany.txt");
                for (int i=0; i<lines.Length; i++)
                {
                    string[] tmp = lines[i].Split(' ');
                    for(int j=0; j<tmp.Length; j++)
                    {
                        try
                        {
                            int liczba = Convert.ToInt32(tmp[j]);
                            streamWriter.Write((Math.Pow(liczba, e) % n) + " ");

                        }
                        catch(Exception)
                        {
                            streamWriter.WriteLine();
                            for (int k=0; k<tmp[j].Length; k++)
                            {
                                int liczba = Convert.ToInt32(tmp[j][k]);
                                streamWriter.Write((Math.Pow(liczba, e) % n) + " ");
                            }
                            
                        }
                    }

                }
                streamWriter.Close();
                MessageBox.Show("Klucz publiczny: (" + e + ";" + n + ")");

            }
            else
            {
                MessageBox.Show("Taki plik nie istnieje!!!");
            }
        }

        private void DekodowanieRSA(string pathFile, int? e, int? n)
        {
            if(File.Exists(pathFile))
            {
                StreamWriter streamWriter;
                streamWriter = File.AppendText(pathFile);
                string[] lines = File.ReadAllLines(pathFile);
                for(int i=0; i<lines.Length; i++)
                {
                    string[] tmp = lines[i].Split(' ');
                    for(int j=0; j<tmp.Length; j++)
                    {
                        try
                        {
                            int c = Convert.ToInt32(tmp[j]);
                            string t = Convert.ToString((Math.Pow(c, (int)e) % n));
                            streamWriter.Write(t);
                        }
                        catch (Exception) { }
                    }
                    streamWriter.WriteLine();
                }
            }
            else
            {
                MessageBox.Show("Nie ma takiego pliku!!!");
            }
        }

        private int NWD(int a, int b)
        {
            int a1 = a;
            int b1 = b;
            int t;

            while(b1 != 0)
            {
                t = b1;
                b1 = a1 % b1;
                a1 = t;
            }

            return a1;
        }

        private int odwMod(int a, int n)
        {
            int a1 = a;
            int n1 = n;
            int q1 = n1 / a1;
            int r = n1 % a1;
            int p0 = 0;
            int p1 = 1;
            int t = 0;
            while (r > 0)
            {
                t = p0 - q1 * p1;
                if (t >= 0)
                    t = t % n;
                else
                    t = n - ((-t) % n);
                p0 = p1;
                p1 = t;
                n1 = a1;
                a1 = r;
                q1 = n1 / a1;
                r = n1 % a1;
            }

            return p1;
        }
    }
}
