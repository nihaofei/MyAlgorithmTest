using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest
{
    public class QRstr
    {
        //二维码中的字符串
        public string str { get; set; }
        //判断两二维码是否属于同一个字符串
        public int num { get; set; }

        //二维码id，分辨是哪个二维码
        public int id { get; set; }
    }

    public class table
    {
        public List<string> str { get; set; }

        public int page { get; set; }

    }
    class Program
    {


        static void Main(string[] args)
        {
            int strsize = 13;//string长度
            int pagesize = 8;//每页数量

            List<string> StrList = new List<string>
            {
                "aaaa","asss","adsdljkasdflkwqefjalfdskjlwqeqwe","dasd","fdslj","alwe","lafljewqljkaljfad","dasf","ljxzc","adsw","ladew","vdsfder","asldjflwqflekfjqw",
                "aaaa","asss","adsdljkasdflkwqefzcxkljvnqwvle,sa","dasdadssssssssssssssssssssssssssssssweeeeeeeeee","fdslj","alwe","lafljewqljkaljfad","dasf","ljxzc","adsw","ladew","vdsfder","asldjflwqflekfjqw",
                "aaaa","asss","adsdljkasdflkwqefczxlkjoiwnrqew","dasd","fdslj","alwe","lafljewqljkaljfad","dasf","ljxzc","adsw","ladew","vdsfder","asldjflwqflekfjqw"
            };

            int id = 0;
            int num = 0;
            List<QRstr> qrstrlist = new List<QRstr>();
            //将数据放到List<QRstr>中
            foreach (var i in StrList)
            {
                num++;
                int run = 0;
                while (i.Length > run)
                {
                    var qrstr = new QRstr();

                    qrstr.str = i.Substring(run, Math.Min(strsize, i.Length - run));//分割长度为13的字符串
                    qrstr.id = id++;
                    qrstr.num = num;
                    qrstrlist.Add(qrstr);
                    run += strsize;
                }
            }

            //qrstrlist数据格式
            //adsw - 23 - 32
            //ladew - 24 - 33
            //vdsfder - 25 - 34
            //asldjflwqflek - 26 - 35
            //fjqw - 26 - 36
            //aaaa - 27 - 37
            //asss - 28 - 38
            //adsdljkasdflk - 29 - 39
            //wqefczxlkjoiw - 29 - 40
            //nrqew - 29 - 41
            //dasd - 30 - 42
            //fdslj - 31 - 43
            //alwe - 32 - 44
            //lafljewqljkal - 33 - 45
            //jfad - 33 - 46
            //dasf - 34 - 47
            //ljxzc - 35 - 48
            //adsw - 36 - 49

            //foreach (var i in qrstrlist)
            //{
            //    Console.WriteLine(i.str + "-" + i.num + "-" + i.id);
            //}


            //根据num将qrstrlist分组，属于同一个字符串的二维码放到一组
            var qrstrlistbynum = qrstrlist.GroupBy(x => x.num).ToList();

            List<int> list = new List<int>();
            //将需求转为：将以下数组进行分组，要求各组之和小于等于8，求最小分组情况。
            //以下代码根据 https://blog.csdn.net/weixin_33894640/article/details/93742399 进行改编
            //1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、2、2、2、2、2、2、3、3、3、4、

            for (var i = 1; i < qrstrlist.Count(); i++)
            {
                var count = qrstrlistbynum.Where(x => x.Count() == i);
                foreach (var j in count)
                {
                    list.Add(i);
                }
            }

            //list数据格式
            //1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、1、2、2、2、2、2、2、3、3、3、4、
            //string teststr = "";

            //foreach (var i in list)
            //{
            //    teststr += i + "、";
            //}
            //Console.WriteLine(teststr);

            var list1 = mainGroup(list, pagesize);
            //list1数据格式
            //[1 - 1 - 1 - 1 - 1 - 1 - 1 - 1 -
            //1 - 1 - 1 - 1 - 1 - 1 - 1 - 1 -
            //1 - 1 - 1 - 1 - 1 - 1 - 1 - 1 -
            //2 - 1 - 1 - 1 - 1 - 1 -
            //2 - 2 - 2 - 2 -
            //3 - 3 - 2 -
            //4 - 3 -

            //foreach (var i in list1)
            //{
            //    string ll = "";
            //    foreach (var ii in i)
            //    {

            //        ll += ii + "-";
            //    }
            //    Console.WriteLine(ll);
            //}

            var tblist = new List<table>();

            int tpage = 0; ;
            //每页的二维码
            foreach (var i in list1)
            {
                tpage++;
                var tb = new table();
                //每个二维码数据
                //根据list1中的数字，查找qrstrlist中对应num的那条数据，将结果记录并从qrstrlist中移除
                var mm = new List<string>();
                foreach (var j in i)
                {
                    int aa = 0;//用以判断是否需要从qrstrlistbynum集合中移除该项

                    for (var d = 1; d <= j; d++)//每个字符串分成了j个二维码
                    {
                        aa++;
                        var qrstrbynum = qrstrlistbynum.Where(x => x.Count() == j).FirstOrDefault();
                        if (qrstrbynum != null)
                        {
                            var qrstrnum = qrstrbynum.Key;
                            var qrstr = qrstrlist.Where(x => x.num == qrstrnum).FirstOrDefault();
                            if (qrstr != null)
                            {

                                mm.Add(qrstr.str);

                            }
                            if (aa == j)
                            {
                                qrstrlistbynum.Remove(qrstrbynum);
                            }

                        }

                    }

                }
                tb.str = mm;
                tb.page = tpage;
                tblist.Add(tb);
            }

            foreach (var i in tblist)
            {
                string aa = "";
                foreach (var j in i.str)
                {
                    aa += j + "-";
                }
                Console.WriteLine("第" + i.page + "页：" + aa);
            }

            Console.ReadKey();


        }


        /// <summary>
        /// 不想写了，看上面的网址，里面有注释
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static List<List<int>> mainGroup(List<int> list, int pagesize)
        {
            List<List<int>> subList = new List<List<int>>();
            List<List<int>> resultList = new List<List<int>>();

            int temp = 0;
            List<int> tempList = new List<int>();

            foreach (var i in list)
            {
                if ((temp + i) < pagesize)
                {
                    tempList.Add(i);
                    temp += i;
                }
                else if ((temp + i) > pagesize)
                {
                    temp = i;
                    subList.Add(tempList);
                    tempList = new List<int>();
                    tempList.Add(i);
                }
                else
                {
                    tempList.Add(i);
                    temp = 0;
                    subList.Add(tempList);
                    tempList = new List<int>();
                }
            }

            subList.Add(tempList);
            for (var m = 0; m < subList.Count(); m++)
            {
                division(subList, resultList, pagesize, m);
            }

            return subList;
        }

        public static void division(List<List<int>> subList, List<List<int>> resultList, int pagesize, int m)
        {
            var maplist = getSortedList(resultList, pagesize, m);

            List<int> subList1 = subList[m];
            subList1.Reverse();

            for (var n = 0; n < subList1.Count(); n++)
            {
                int i = subList1[n];
                for (var j = 0; j < maplist.Count(); j++)
                {
                    Dictionary<int, List<int>> listMap = maplist[j];
                    int diTemp = listMap.Select(x => x.Key).FirstOrDefault();
                    List<int> listTemp = listMap[diTemp];
                    if (i < diTemp)
                    {
                        continue;
                    }
                    else
                    {
                        listMap.Remove(diTemp);
                        diTemp -= i;
                        listTemp.Add(i);
                        listMap.Add(diTemp, listTemp);
                        subList1[n] = 0;
                        DictionarySort(maplist);
                        break;
                    }
                }
            }
        }

        public static List<Dictionary<int, List<int>>> getSortedList(List<List<int>> subList, int k, int m)
        {
            List<Dictionary<int, List<int>>> mapList = new List<Dictionary<int, List<int>>>();
            for (var i = 0; i < subList.Count(); i++)
            {
                Dictionary<int, List<int>> tempMap = new Dictionary<int, List<int>>();
                tempMap.Add((k - ListSum(subList[i])), subList[i]);

            }
            DictionarySort(mapList);
            return mapList;
        }



        private static void DictionarySort(List<Dictionary<int, List<int>>> mapList)
        {
            var array = mapList.ToArray();
            Array.Sort(array);

        }

        public static int ListSum(List<int> list)
        {
            int sum = 0;
            foreach (var i in list)
            {
                sum += i;
            }
            return sum;
        }
    }
}
