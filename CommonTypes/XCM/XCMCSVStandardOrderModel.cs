using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes.XCM
{

	public class XCMCSVHeader
	{
        public string SegmentName { get; set; } //1

		public string Reference { get; set; } //2

		public string Reference2 { get; set; } //3

		public string HeaderInfo2 { get; set; } //4

		public string RegTypeID { get; set; } //5

		public string HeaderInfo3 { get; set; } //6

		public string HeaderInfo4 { get; set; } //7

		public string UnloadName { get; set; } //8

		public string UnloadZipCode { get; set; } //9

		public string UnloadLocation { get; set; } //10

		public string UnloadAddress { get; set; } //11

		public string UnloadCountry { get; set; } //12

		public string UnloadDistrict { get; set; } //13

		public string RefDta { get; set; } //14

		public string RefDta2 { get; set; } //15

		public string HeaderInfo5 { get; set; } //16

		public override string ToString()
		{
			return $"{SegmentName};{Reference};{Reference2};{HeaderInfo2};{RegTypeID};{HeaderInfo3};{HeaderInfo4};{UnloadName};{UnloadZipCode};{UnloadLocation};{UnloadAddress};{UnloadCountry};{UnloadDistrict};{RefDta};{RefDta2};{HeaderInfo5}";
		}
	}

	public class XCMCSVRow
	{
		public string RowInfo1 { get; set; } //2

		public string PrdCod { get; set; } //3

		public string Qty { get; set; } //4

		public string Batchno { get; set; } //5

		public string DateExpire { get; set; } //6

		public string DateProd { get; set; } //7

		public string RowInfo2 { get; set; } //8

		public string RowInfo3 { get; set; } //9


		public override string ToString()
        {
            return $"ROW;{RowInfo1};{PrdCod};{Qty};{Batchno};{DateExpire};{DateProd};{RowInfo2};{RowInfo3}";
        }

	}
}
