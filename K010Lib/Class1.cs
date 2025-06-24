using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace K010Lib
{
    [TestFixture]
    public class Class1
    {
        int age;
        string name;
        string email;

        enum Mua { Xuan=1, Ha=2, Thu=3, Dong=4};


        [Test]
        public void TinhCong()
        {
            int a = 20;
            int b = 20;
            bool c = (a>=b);

            bool d1 = false;
            bool d2 = false;
            bool d3 = !d1;

            Console.WriteLine("c = " + d3);

            int SoNguyen1=10;
            long SoNguyen2 = SoNguyen1;
            int SoNguyen3 = (int)SoNguyen2;
        }


        [Test]
        public void CauLenhIf()
        {
            int Diem = 80;
            if (Diem >= 85)
            {
                Console.WriteLine("Xếp loại giỏi");
            }   
            else if (Diem >= 70)
            {
                Console.WriteLine("Xếp loại Khá");
            }
            else if (Diem >= 50)
            {
                Console.WriteLine("Xếp loại Trung bình");
            }
            else
            {
                Console.WriteLine("Xếp loại yếu");
            }    
        }


        [Test]
        public void CauLenhSwitch()
        {
            int LoaiXe =5;
            
            switch (LoaiXe)
            {
                case 1:
                    {
                        Console.WriteLine("Xe Đạp");
                    }
                    break;

                case 2:
                    {
                        Console.WriteLine("Xe Máy");
                    }
                    break;

                case 3:
                    {
                        Console.WriteLine("Ô tô");
                    }
                    break;

                default:
                    {
                        Console.WriteLine("Lựa chọn không hợp lệ");
                    }
                    break;
            }    
        }


        [Test]
        public void VongLapFor()
        {
            int SoLanLap = 5;

            for (int i=1;i<=5;i++)
            {
                Console.WriteLine("Gửi Email lần thứ: " + i);
            }    
        }

        [Test]
        public void VongLapForEach()
        {
            string[] DanhSachTen = { "Hùng", "Tuấn", "Toàn", "ShiSu", "Nhựt", "Cường", "Huy" };
            
            foreach (string Ten in DanhSachTen)
            {
                Console.WriteLine("Tên: " + Ten);
            }
        }


        [Test]
        public void VongLapWhile()
        {
            int Count = 5;

            do
            {
                Console.WriteLine("Đây là vòng lặp While lần thứ: " + Count);
                Count++;
            }
            while (Count < 5) ;
        }

        [Test]
        public void CuPhapBreak()
        {
            int SoLanLap = 5;

            for (int i = 1; i <= 20; i++)
            {
                Console.WriteLine("Gửi Email lần thứ: " + i);
                if (i == 10)
                {
                    break;
                }    
            }
        }

        [Test]
        public void CuPhapCont()
        {
            for (int i = 1; i <= 20; i++)
            {
                if (i == 10 || i == 15)
                {
                    continue;
                }
                Console.WriteLine("Gửi Email lần thứ: " + i);
            }
        }

        [Test]
        public void TinhCong2()
        {
            int a = 20;
            int b = 20;
            bool c = (a >= b);

            bool d1 = false;
            bool d2 = false;
            bool d3 = !d1;

            Console.WriteLine("c = " + d3);

            int SoNguyen1 = 10;
            long SoNguyen2 = SoNguyen1;
            int SoNguyen3 = (int)SoNguyen2;
        }

        [Test]
        public void DoiTuong()
        {
            clNguoi OngCuong = new clNguoi();
            OngCuong.Name = "Cường";
            OngCuong.Age = 30;
            OngCuong.Email = "cuong@gmail.com";
            OngCuong.NoiChuyen();

            clNguoi ChiHuy = new clNguoi();
            ChiHuy.Name = "Chí Huy";
            ChiHuy.Age = OngCuong.Age;
            ChiHuy.Email = "chihuy@gmail.com";
            ChiHuy.NoiChuyen();

            OngCuong.NoiChuyen();

            int a = 20;
            Console.WriteLine("Xin chào, tôi là " + a);
            int b = a;
            b = 10;
            Console.WriteLine("Xin chào, tôi là " + b);
            Console.WriteLine("Xin chào, tôi là " + a);

            OngCuong = null;
            ChiHuy = null;
        }
    }
}
