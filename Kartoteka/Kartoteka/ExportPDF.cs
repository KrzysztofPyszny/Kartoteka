using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka
{
    class ExportPDF
    {
        public static void ExportDataSetToPdf(DataSet dsTable, String strPdfPath, string strHeader)
        {
            System.IO.FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            BaseFont bf = BaseFont.CreateFont("C:/WINDOWS/FONTS/TIMES.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font fntHead = new Font(bf, 20, 1, iTextSharp.text.BaseColor.GRAY);
            iTextSharp.text.Paragraph prgHeading = new iTextSharp.text.Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
            document.Add(prgHeading);

            document.Add(new Chunk("\n", fntHead));

            iTextSharp.text.Paragraph prgAuthor = new iTextSharp.text.Paragraph();
            Font fntAuthor = new Font(bf, 10, 2, iTextSharp.text.BaseColor.GRAY);
            prgAuthor.Alignment = Element.ALIGN_RIGHT;
            prgAuthor.Add(new Chunk("Autor: Adam Posłuszny", fntAuthor));
            prgAuthor.Add(new Chunk("\n Data utworzenia: " + DateTime.Now.ToShortDateString(), fntAuthor));
            prgAuthor.Add(new Chunk("\n nr PWZF 1266", fntAuthor));
            document.Add(prgAuthor);

            document.Add(new Chunk("\n", fntHead));

            Font fontDane = new Font(bf, 12);

            document.Add(new Chunk("Imię nazwisko: " + dsTable.Tables["PacjenciOsobowe"].Rows[0][1].ToString() + " " + dsTable.Tables["PacjenciOsobowe"].Rows[0][2].ToString() + "\n", fontDane));
            document.Add(new Chunk("PESEL: " + dsTable.Tables["PacjenciOsobowe"].Rows[0][6].ToString() + "\n", fontDane));

            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, 0, 0.0F)));
            document.Add(p);


            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Dane medyczne: \n", fontDane));

            PdfPTable tableMedyczne = new PdfPTable(6);
            tableMedyczne.WidthPercentage = 95;

            PdfPCell cellTproblemyWciazy = new PdfPCell(new Phrase("Problemy w trakcie ciąży", fontDane));
            cellTproblemyWciazy.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTproblemyWciazy.VerticalAlignment = Element.ALIGN_CENTER;
            cellTproblemyWciazy.Padding = 5;
            tableMedyczne.AddCell(cellTproblemyWciazy);
            PdfPCell cellProblemyWciazy = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][1].ToString(), fontDane));
            cellProblemyWciazy.Padding = 5;
            cellProblemyWciazy.Colspan = 5;
            tableMedyczne.AddCell(cellProblemyWciazy);


            PdfPCell cellTporod = new PdfPCell(new Phrase("Poród", fontDane));
            cellTporod.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTporod.VerticalAlignment = Element.ALIGN_CENTER;
            cellTporod.Padding = 5;
            cellTporod.Rowspan = 2;
            tableMedyczne.AddCell(cellTporod);

            PdfPCell cellTwTerminie = new PdfPCell(new Phrase("w terminie", fontDane));
            cellTwTerminie.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTwTerminie.VerticalAlignment = Element.ALIGN_CENTER;
            cellTwTerminie.Padding = 5;
            tableMedyczne.AddCell(cellTwTerminie);
            PdfPCell cellTpozaTerminem = new PdfPCell(new Phrase("poza terminie", fontDane));
            cellTpozaTerminem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozaTerminem.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozaTerminem.Padding = 5;
            tableMedyczne.AddCell(cellTpozaTerminem);
            PdfPCell cellTsilamiNatury = new PdfPCell(new Phrase("siłami natury", fontDane));
            cellTsilamiNatury.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTsilamiNatury.VerticalAlignment = Element.ALIGN_CENTER;
            cellTsilamiNatury.Padding = 5;
            tableMedyczne.AddCell(cellTsilamiNatury);
            PdfPCell cellTcesarskieCieciePlanowane = new PdfPCell(new Phrase("cesarskie cięcie planowane", fontDane));
            cellTcesarskieCieciePlanowane.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTcesarskieCieciePlanowane.VerticalAlignment = Element.ALIGN_CENTER;
            cellTcesarskieCieciePlanowane.Padding = 5;
            tableMedyczne.AddCell(cellTcesarskieCieciePlanowane);
            PdfPCell cellTcesarskieCiecieZkoniecznosci = new PdfPCell(new Phrase("cesarskie cięcie z konieczności", fontDane));
            cellTcesarskieCiecieZkoniecznosci.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTcesarskieCiecieZkoniecznosci.VerticalAlignment = Element.ALIGN_CENTER;
            cellTcesarskieCiecieZkoniecznosci.Padding = 5;
            tableMedyczne.AddCell(cellTcesarskieCiecieZkoniecznosci);

            PdfPCell cellwTerminie = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][2].ToString(), fontDane));
            cellwTerminie.HorizontalAlignment = Element.ALIGN_CENTER;
            cellwTerminie.VerticalAlignment = Element.ALIGN_CENTER;
            cellwTerminie.Padding = 5;
            tableMedyczne.AddCell(cellwTerminie);
            PdfPCell cellpozaTerminem = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][3].ToString(), fontDane));
            cellpozaTerminem.HorizontalAlignment = Element.ALIGN_CENTER;
            cellpozaTerminem.VerticalAlignment = Element.ALIGN_CENTER;
            cellpozaTerminem.Padding = 5;
            tableMedyczne.AddCell(cellpozaTerminem);
            PdfPCell cellsilamiNatury = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][4].ToString(), fontDane));
            cellsilamiNatury.HorizontalAlignment = Element.ALIGN_CENTER;
            cellsilamiNatury.VerticalAlignment = Element.ALIGN_CENTER;
            cellsilamiNatury.Padding = 5;
            tableMedyczne.AddCell(cellsilamiNatury);
            PdfPCell cellcesarskieCieciePlanowane = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][5].ToString(), fontDane));
            cellcesarskieCieciePlanowane.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcesarskieCieciePlanowane.VerticalAlignment = Element.ALIGN_CENTER;
            cellcesarskieCieciePlanowane.Padding = 5;
            tableMedyczne.AddCell(cellcesarskieCieciePlanowane);
            PdfPCell cellcesarskieCiecieZkoniecznosci = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][6].ToString(), fontDane));
            cellcesarskieCiecieZkoniecznosci.HorizontalAlignment = Element.ALIGN_CENTER;
            cellcesarskieCiecieZkoniecznosci.VerticalAlignment = Element.ALIGN_CENTER;
            cellcesarskieCiecieZkoniecznosci.Padding = 5;
            tableMedyczne.AddCell(cellcesarskieCiecieZkoniecznosci);


            PdfPCell cellTproblemyPoPorodzie = new PdfPCell(new Phrase("Problemy po porodzie", fontDane));
            cellTproblemyPoPorodzie.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTproblemyPoPorodzie.VerticalAlignment = Element.ALIGN_CENTER;
            cellTproblemyPoPorodzie.Padding = 5;
            tableMedyczne.AddCell(cellTproblemyPoPorodzie);
            PdfPCell cellproblemyPoPorodzie = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][7].ToString(), fontDane));
            cellproblemyPoPorodzie.Padding = 5;
            cellproblemyPoPorodzie.Colspan = 5;
            tableMedyczne.AddCell(cellproblemyPoPorodzie);

            PdfPCell cellTdaneUzyskane = new PdfPCell(new Phrase("Dane uzyskane z wypisów i badań medycznych", fontDane));
            cellTdaneUzyskane.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTdaneUzyskane.VerticalAlignment = Element.ALIGN_CENTER;
            cellTdaneUzyskane.Padding = 5;
            tableMedyczne.AddCell(cellTdaneUzyskane);
            PdfPCell celldaneUzyskane = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][8].ToString(), fontDane));
            celldaneUzyskane.Padding = 5;
            celldaneUzyskane.Colspan = 5;
            tableMedyczne.AddCell(celldaneUzyskane);

            PdfPCell cellTzauwaznoeProblemy = new PdfPCell(new Phrase("Problemy zauważone przez rodziców", fontDane));
            cellTzauwaznoeProblemy.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTzauwaznoeProblemy.VerticalAlignment = Element.ALIGN_CENTER;
            cellTzauwaznoeProblemy.Padding = 5;
            tableMedyczne.AddCell(cellTzauwaznoeProblemy);
            PdfPCell cellzauwaznoeProblemy = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][9].ToString(), fontDane));
            cellzauwaznoeProblemy.Padding = 5;
            cellzauwaznoeProblemy.Colspan = 5;
            tableMedyczne.AddCell(cellzauwaznoeProblemy);

            PdfPCell cellTinfoPrzetwarzanie = new PdfPCell(new Phrase("Informacje dotyczące przetwarzania sensorycznego", fontDane));
            cellTinfoPrzetwarzanie.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTinfoPrzetwarzanie.VerticalAlignment = Element.ALIGN_CENTER;
            cellTinfoPrzetwarzanie.Padding = 5;
            tableMedyczne.AddCell(cellTinfoPrzetwarzanie);
            PdfPCell cellinfoPrzetwarzanie = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][10].ToString(), fontDane));
            cellinfoPrzetwarzanie.Padding = 5;
            cellinfoPrzetwarzanie.Colspan = 5;
            tableMedyczne.AddCell(cellinfoPrzetwarzanie);

            PdfPCell cellTinneSchorzenia = new PdfPCell(new Phrase("Inne schorzenia, alergie, epilepsja, zabiegi", fontDane));
            cellTinneSchorzenia.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTinneSchorzenia.VerticalAlignment = Element.ALIGN_CENTER;
            cellTinneSchorzenia.Padding = 5;
            tableMedyczne.AddCell(cellTinneSchorzenia);
            PdfPCell cellinneSchorzenia = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][11].ToString(), fontDane));
            cellinneSchorzenia.Padding = 5;
            cellinneSchorzenia.Colspan = 5;
            tableMedyczne.AddCell(cellinneSchorzenia);

            document.Add(tableMedyczne);

            document.NewPage();

            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Obserwacje: \n", fontDane));

            PdfPTable tableObserwacje = new PdfPTable(6);
            tableObserwacje.WidthPercentage = 95;

            PdfPCell cellTpozycjaSupinacyjna = new PdfPCell(new Phrase("Pozycja supinacyjna", fontDane));
            cellTpozycjaSupinacyjna.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaSupinacyjna.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaSupinacyjna.Padding = 5;
            tableObserwacje.AddCell(cellTpozycjaSupinacyjna);
            PdfPCell cellpozycjaSupinacyjna = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][12].ToString(), fontDane));
            cellpozycjaSupinacyjna.Padding = 5;
            cellpozycjaSupinacyjna.Colspan = 5;
            tableObserwacje.AddCell(cellpozycjaSupinacyjna);

            PdfPCell cellTpozycjaPronacyjna = new PdfPCell(new Phrase("Pozycja pronacyjna", fontDane));
            cellTpozycjaPronacyjna.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaPronacyjna.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaPronacyjna.Padding = 5;
            tableObserwacje.AddCell(cellTpozycjaPronacyjna);
            PdfPCell cellpozycjaPronacyjna = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][13].ToString(), fontDane));
            cellpozycjaPronacyjna.Padding = 5;
            cellpozycjaPronacyjna.Colspan = 5;
            tableObserwacje.AddCell(cellpozycjaPronacyjna);

            PdfPCell cellTpozycjaBocznaL = new PdfPCell(new Phrase("Pozycja boczna lewa", fontDane));
            cellTpozycjaBocznaL.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaBocznaL.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaBocznaL.Padding = 5;
            tableObserwacje.AddCell(cellTpozycjaBocznaL);
            PdfPCell cellpozycjaBocznaL = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][14].ToString(), fontDane));
            cellpozycjaBocznaL.Padding = 5;
            cellpozycjaBocznaL.Colspan = 5;
            tableObserwacje.AddCell(cellpozycjaBocznaL);

            PdfPCell cellTpozycjaBocznaP = new PdfPCell(new Phrase("Pozycja boczna prawa", fontDane));
            cellTpozycjaBocznaP.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaBocznaP.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaBocznaP.Padding = 5;
            tableObserwacje.AddCell(cellTpozycjaBocznaP);
            PdfPCell cellpozycjaBocznaP = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][15].ToString(), fontDane));
            cellpozycjaBocznaP.Padding = 5;
            cellpozycjaBocznaP.Colspan = 5;
            tableObserwacje.AddCell(cellpozycjaBocznaP);

            PdfPCell cellTpozycjaSiedzaca = new PdfPCell(new Phrase("Pozycja siedząca", fontDane));
            cellTpozycjaSiedzaca.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaSiedzaca.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaSiedzaca.Padding = 5;
            tableObserwacje.AddCell(cellTpozycjaSiedzaca);
            PdfPCell cellpozycjaSiedzaca = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][16].ToString(), fontDane));
            cellpozycjaSiedzaca.Padding = 5;
            cellpozycjaSiedzaca.Colspan = 5;
            tableObserwacje.AddCell(cellpozycjaSiedzaca);

            PdfPCell cellTklekPodparty = new PdfPCell(new Phrase("Klęk podparty", fontDane));
            cellTklekPodparty.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTklekPodparty.VerticalAlignment = Element.ALIGN_CENTER;
            cellTklekPodparty.Padding = 5;
            tableObserwacje.AddCell(cellTklekPodparty);
            PdfPCell cellklekPodparty = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][17].ToString(), fontDane));
            cellklekPodparty.Padding = 5;
            cellklekPodparty.Colspan = 5;
            tableObserwacje.AddCell(cellklekPodparty);

            PdfPCell cellTklekJednonoz = new PdfPCell(new Phrase("Klęk jednonóż", fontDane));
            cellTklekJednonoz.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTklekJednonoz.VerticalAlignment = Element.ALIGN_CENTER;
            cellTklekJednonoz.Padding = 5;
            tableObserwacje.AddCell(cellTklekJednonoz);
            PdfPCell cellklekJednonoz = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][18].ToString(), fontDane));
            cellklekJednonoz.Padding = 5;
            cellklekJednonoz.Colspan = 5;
            tableObserwacje.AddCell(cellklekJednonoz);

            PdfPCell cellTpozycjaStojaca = new PdfPCell(new Phrase("Pozycja stojąca", fontDane));
            cellTpozycjaStojaca.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaStojaca.VerticalAlignment = Element.ALIGN_CENTER;
            cellTpozycjaStojaca.Padding = 5;
            tableObserwacje.AddCell(cellTpozycjaStojaca);
            PdfPCell cellpozycjaStojaca = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][19].ToString(), fontDane));
            cellpozycjaStojaca.Padding = 5;
            cellpozycjaStojaca.Colspan = 5;
            tableObserwacje.AddCell(cellpozycjaStojaca);

            PdfPCell cellTchod = new PdfPCell(new Phrase("Chód", fontDane));
            cellTchod.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTchod.VerticalAlignment = Element.ALIGN_CENTER;
            cellTchod.Padding = 5;
            tableObserwacje.AddCell(cellTchod);
            PdfPCell cellchod = new PdfPCell(new Phrase(dsTable.Tables["PacjenciMedyczne"].Rows[0][20].ToString(), fontDane));
            cellchod.Padding = 5;
            cellchod.Colspan = 5;
            tableObserwacje.AddCell(cellchod);

            document.Add(tableObserwacje);

            document.NewPage();

            document.Add(new Chunk("\n"));
            document.Add(new Chunk("Wizyty: \n", fontDane));


            for (int i = 0; i < dsTable.Tables["Wizyty"].Rows.Count; i++)
            {
                if (i % 2 == 0 && i != 0)
                {
                    document.NewPage();
                }
                PdfPTable tableWizyty = new PdfPTable(7);
                tableWizyty.WidthPercentage = 95;

                PdfPCell cellTdata = new PdfPCell(new Phrase("Data wizyty", fontDane));
                cellTdata.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTdata.VerticalAlignment = Element.ALIGN_CENTER;
                cellTdata.Padding = 5;
                tableWizyty.AddCell(cellTdata);

                PdfPCell cellTZmiany = new PdfPCell(new Phrase("Zmiany", fontDane));
                cellTZmiany.Colspan = 3;
                cellTZmiany.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTZmiany.VerticalAlignment = Element.ALIGN_CENTER;
                cellTZmiany.Padding = 5;
                tableWizyty.AddCell(cellTZmiany);

                PdfPCell cellTZalecenia = new PdfPCell(new Phrase("Zalecenia", fontDane));
                cellTZalecenia.Colspan = 3;
                cellTZalecenia.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTZalecenia.VerticalAlignment = Element.ALIGN_CENTER;
                cellTZalecenia.Padding = 5;
                tableWizyty.AddCell(cellTZalecenia);

                PdfPCell celldata = new PdfPCell(new Phrase(Convert.ToDateTime(dsTable.Tables["Wizyty"].Rows[i][1]).ToShortDateString(), fontDane));
                celldata.Padding = 5;
                tableWizyty.AddCell(celldata);

                PdfPCell cellZmiany = new PdfPCell(new Phrase(dsTable.Tables["Wizyty"].Rows[i][2].ToString(), fontDane));
                cellZmiany.Colspan = 3;
                cellZmiany.Padding = 5;
                tableWizyty.AddCell(cellZmiany);

                PdfPCell cellZalecenia = new PdfPCell(new Phrase(dsTable.Tables["Wizyty"].Rows[i][3].ToString(), fontDane));
                cellZalecenia.Colspan = 3;
                cellZalecenia.Padding = 5;
                tableWizyty.AddCell(cellZalecenia);

                PdfPCell cellTZabiegi = new PdfPCell(new Phrase("Zabiegi", fontDane));
                cellTZabiegi.Colspan = 7;
                cellTZabiegi.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTZabiegi.VerticalAlignment = Element.ALIGN_CENTER;
                cellTZabiegi.Padding = 5;
                tableWizyty.AddCell(cellTZabiegi);

                PdfPCell cellZabiegi = new PdfPCell(new Phrase(dsTable.Tables["Wizyty"].Rows[i][4].ToString(), fontDane));
                cellZabiegi.Colspan = 7;
                cellZabiegi.Padding = 5;
                tableWizyty.AddCell(cellZabiegi);

                document.Add(tableWizyty);
                document.Add(new Chunk("\n"));
            }

            document.Close();
            writer.Close();
            fs.Close();
        }
    }
}
