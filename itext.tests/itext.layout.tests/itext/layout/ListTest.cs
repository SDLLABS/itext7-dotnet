/*
This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV
Authors: iText Software.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.Collections.Generic;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Utils;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Layout {
    public class ListTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/ListTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/ListTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NestedListTest01() {
            String outFileName = destinationFolder + "nestedListTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_nestedListTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            List romanList2 = new List(ListNumberingType.ROMAN_LOWER).SetSymbolIndent(20).SetMarginLeft(25).Add("One")
                .Add("Two").Add("Three");
            List romanList = new List(ListNumberingType.ROMAN_LOWER).SetSymbolIndent(20).SetMarginLeft(25).Add("One").
                Add("Two").Add((ListItem)new ListItem("Three").Add(romanList2));
            List list = new List(ListNumberingType.DECIMAL).SetSymbolIndent(20).Add("One").Add("Two").Add("Three").Add
                ("Four").Add((ListItem)new ListItem("Roman List").Add(romanList)).Add("Five").Add("Six").Add((ListItem
                )new ListItem().Add(romanList2));
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NestedListTest02() {
            String outFileName = destinationFolder + "nestedListTest02.pdf";
            String cmpFileName = sourceFolder + "cmp_nestedListTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            List nestedList = new List().SetSymbolIndent(20).SetMarginLeft(25).Add("One").Add("Two").Add("Three");
            List list = new List(ListNumberingType.DECIMAL).SetSymbolIndent(20).Add("One").Add("Two").Add("Three").Add
                ("Four").Add((ListItem)new ListItem().Add(nestedList));
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListNestedInTableTest01() {
            String outFileName = destinationFolder + "listNestedInTableTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_listNestedInTableTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument, PageSize.A9.Rotate());
            List list = new List(ListNumberingType.DECIMAL).Add("first string").Add("second string").Add("third string"
                ).Add("fourth string");
            Table table = new Table(1);
            table.AddCell(new Cell().Add(list).SetVerticalAlignment(VerticalAlignment.BOTTOM));
            document.Add(table);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListNumberingTest01() {
            String outFileName = destinationFolder + "listNumberingTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_listNumberingTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            IList<List> lists = new List<List>();
            lists.Add(new List(ListNumberingType.DECIMAL));
            lists.Add(new List(ListNumberingType.ROMAN_LOWER));
            lists.Add(new List(ListNumberingType.ROMAN_UPPER));
            lists.Add(new List(ListNumberingType.ENGLISH_LOWER));
            lists.Add(new List(ListNumberingType.ENGLISH_UPPER));
            lists.Add(new List(ListNumberingType.GREEK_LOWER));
            lists.Add(new List(ListNumberingType.GREEK_UPPER));
            for (int i = 1; i <= 30; i++) {
                foreach (List list in lists) {
                    list.Add("Item #" + i);
                }
            }
            foreach (List list in lists) {
                document.Add(list).Add(new AreaBreak());
            }
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.ELEMENT_DOES_NOT_FIT_AREA, Count = 8)]
        public virtual void AddListOnShortPage1() {
            String outFileName = destinationFolder + "addListOnShortPage1.pdf";
            String cmpFileName = sourceFolder + "cmp_addListOnShortPage1.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFileName));
            Document doc = new Document(pdfDoc, new PageSize(500, 60));
            ListItem item = new ListItem();
            ListItem nestedItem = new ListItem();
            List list = new List(ListNumberingType.DECIMAL);
            List nestedList = new List(ListNumberingType.ENGLISH_UPPER);
            List nestedNestedList = new List(ListNumberingType.GREEK_LOWER);
            nestedNestedList.Add("Hello");
            nestedNestedList.Add("World");
            nestedItem.Add(nestedNestedList);
            nestedList.Add(nestedItem);
            nestedList.Add(nestedItem);
            item.Add(nestedList);
            list.Add(item);
            list.Add(item);
            doc.Add(list);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(iText.IO.LogMessageConstant.ELEMENT_DOES_NOT_FIT_AREA, Count = 3)]
        public virtual void AddListOnShortPage2() {
            String outFileName = destinationFolder + "addListOnShortPage2.pdf";
            String cmpFileName = sourceFolder + "cmp_addListOnShortPage2.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFileName));
            Document doc = new Document(pdfDoc, new PageSize(500, 130));
            List list = new List(ListNumberingType.DECIMAL);
            ListItem item = new ListItem();
            item.Add(new Paragraph("Red"));
            item.Add(new Paragraph("Is"));
            item.Add(new Paragraph("The"));
            item.Add(new Paragraph("Color"));
            item.Add(new Image(ImageDataFactory.Create(sourceFolder + "red.png")));
            List nestedList = new List(ListNumberingType.ENGLISH_UPPER);
            nestedList.Add("Hello");
            nestedList.Add("World");
            item.Add(nestedList);
            for (int i = 0; i < 3; i++) {
                list.Add(item);
            }
            doc.Add(list);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DivInListItemTest01() {
            String outFileName = destinationFolder + "divInListItemTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_divInListItemTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            ListItem item = new ListItem();
            item.Add(new Div().Add(new Paragraph("text")));
            document.Add(new List().Add(item));
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListOverflowTest01() {
            String outFileName = destinationFolder + "listOverflowTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_listOverflowTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            Paragraph p = new Paragraph("Test String");
            List list = new List(ListNumberingType.DECIMAL).Add("first string").Add("second string").Add("third string"
                ).Add("fourth string");
            for (int i = 0; i < 28; i++) {
                document.Add(p);
            }
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListOverflowTest02() {
            String outFileName = destinationFolder + "listOverflowTest02.pdf";
            String cmpFileName = sourceFolder + "cmp_listOverflowTest02.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            Paragraph p = new Paragraph("Test String");
            List list = new List(ListNumberingType.DECIMAL).Add("first string");
            ListItem item = (ListItem)new ListItem("second string").Add(new Paragraph("third string"));
            list.Add(item).Add("fourth item");
            for (int i = 0; i < 28; i++) {
                document.Add(p);
            }
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListOverflowTest03() {
            String outFileName = destinationFolder + "listOverflowTest03.pdf";
            String cmpFileName = sourceFolder + "cmp_listOverflowTest03.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            Paragraph p = new Paragraph("Test String");
            List list = new List(ListNumberingType.DECIMAL).SetItemStartIndex(10).Add("first string").Add("second string"
                ).Add("third string").Add("fourth string");
            for (int i = 0; i < 28; i++) {
                document.Add(p);
            }
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListEmptyItemTest01() {
            String outFileName = destinationFolder + "listEmptyItemTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_listEmptyItemTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            List list = new List(ListNumberingType.GREEK_LOWER);
            list.Add(new ListItem()).Add(new ListItem()).Add(new ListItem()).Add("123").Add((ListItem)new ListItem().Add
                (new Div()));
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ImageInListTest01() {
            String outFileName = destinationFolder + "imageInListTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_imageInListTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            List list = new List(ListNumberingType.GREEK_LOWER);
            PdfImageXObject xObject = new PdfImageXObject(ImageDataFactory.Create(sourceFolder + "Desert.jpg"));
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(xObject, 100);
            list.Add(new ListItem()).Add(new ListItem(image)).Add(new ListItem()).Add("123").Add((ListItem)new ListItem
                ().Add(new Div().SetHeight(70).SetBackgroundColor(Color.RED)));
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListItemAlignmentTest01() {
            String outFileName = destinationFolder + "listItemAlignmentTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_listItemAlignmentTest01.pdf";
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdfDocument);
            List list = new List(ListNumberingType.DECIMAL).SetListSymbolAlignment(ListSymbolAlignment.LEFT);
            for (int i = 1; i <= 30; i++) {
                list.Add("Item #" + i);
            }
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListItemTest01() {
            String outFileName = destinationFolder + "listItemTest01.pdf";
            String cmpFileName = sourceFolder + "cmp_listItemTest01.pdf";
            PdfDocument pdf = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdf);
            List list = new List();
            list.Add(new ListItem("The quick brown").SetListSymbol(ListNumberingType.ZAPF_DINGBATS_1)).Add(new ListItem
                ("fox").SetListSymbol(ListNumberingType.ZAPF_DINGBATS_2)).Add(new ListItem("jumps over the lazy").SetListSymbol
                (ListNumberingType.ZAPF_DINGBATS_3)).Add(new ListItem("dog").SetListSymbol(ListNumberingType.ZAPF_DINGBATS_4
                ));
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListItemTest02() {
            String outFileName = destinationFolder + "listItemTest02.pdf";
            String cmpFileName = sourceFolder + "cmp_listItemTest02.pdf";
            PdfDocument pdf = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdf);
            document.SetFontColor(Color.WHITE);
            List list = new List();
            Style liStyle = new Style().SetMargins(20, 0, 20, 0).SetBackgroundColor(Color.BLACK);
            list.Add((ListItem)new ListItem("").AddStyle(liStyle)).Add((ListItem)new ListItem("fox").AddStyle(liStyle)
                ).Add((ListItem)new ListItem("").AddStyle(liStyle)).Add((ListItem)new ListItem("dog").AddStyle(liStyle
                ));
            document.Add(list.SetBackgroundColor(Color.BLUE));
            document.Add(new Paragraph("separation between lists"));
            liStyle.SetMargin(0);
            document.Add(list);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [LogMessage(iText.IO.LogMessageConstant.CLIP_ELEMENT, Count = 4)]
        [NUnit.Framework.Test]
        public virtual void ListWithSetHeightProperties01() {
            String outFileName = destinationFolder + "listWithSetHeightProperties01.pdf";
            String cmpFileName = sourceFolder + "cmp_listWithSetHeightProperties01.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFileName));
            Document doc = new Document(pdfDoc);
            doc.Add(new Paragraph("Default layout:"));
            ListItem item = new ListItem();
            ListItem nestedItem = new ListItem();
            List list = new List(ListNumberingType.DECIMAL);
            List nestedList = new List(ListNumberingType.ENGLISH_UPPER);
            List nestedNestedList = new List(ListNumberingType.GREEK_LOWER);
            nestedNestedList.Add("Hello");
            nestedNestedList.Add("World");
            nestedItem.Add(nestedNestedList);
            nestedList.Add(nestedItem);
            nestedList.Add(nestedItem);
            item.Add(nestedList);
            list.Add(item);
            list.Add(item);
            list.SetBorder(new SolidBorder(Color.RED, 3));
            doc.Add(list);
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("List's height is set shorter than needed:"));
            list.SetHeight(50);
            doc.Add(list);
            doc.Add(new AreaBreak());
            list.DeleteOwnProperty(Property.HEIGHT);
            list.DeleteOwnProperty(Property.MIN_HEIGHT);
            list.DeleteOwnProperty(Property.MAX_HEIGHT);
            doc.Add(new Paragraph("List's min height is set shorter than needed:"));
            list.SetMinHeight(50);
            doc.Add(list);
            doc.Add(new AreaBreak());
            list.DeleteOwnProperty(Property.HEIGHT);
            list.DeleteOwnProperty(Property.MIN_HEIGHT);
            list.DeleteOwnProperty(Property.MAX_HEIGHT);
            doc.Add(new Paragraph("List's max height is set shorter than needed:"));
            list.SetMaxHeight(50);
            doc.Add(list);
            doc.Add(new AreaBreak());
            list.DeleteOwnProperty(Property.HEIGHT);
            list.DeleteOwnProperty(Property.MIN_HEIGHT);
            list.DeleteOwnProperty(Property.MAX_HEIGHT);
            doc.Add(new Paragraph("List's height is set bigger than needed:"));
            list.SetHeight(1300);
            doc.Add(list);
            doc.Add(new AreaBreak());
            list.DeleteOwnProperty(Property.HEIGHT);
            list.DeleteOwnProperty(Property.MIN_HEIGHT);
            list.DeleteOwnProperty(Property.MAX_HEIGHT);
            doc.Add(new Paragraph("List's min height is set bigger than needed:"));
            list.SetMinHeight(1300);
            doc.Add(list);
            doc.Add(new AreaBreak());
            list.DeleteOwnProperty(Property.HEIGHT);
            list.DeleteOwnProperty(Property.MIN_HEIGHT);
            list.DeleteOwnProperty(Property.MAX_HEIGHT);
            doc.Add(new Paragraph("List's max height is set bigger than needed:"));
            list.SetMaxHeight(1300);
            doc.Add(list);
            doc.Add(new AreaBreak());
            list.DeleteOwnProperty(Property.HEIGHT);
            list.DeleteOwnProperty(Property.MIN_HEIGHT);
            list.DeleteOwnProperty(Property.MAX_HEIGHT);
            doc.Add(new Paragraph("Some list items' and nested lists' heights are set bigger or shorter than needed:")
                );
            nestedList.SetHeight(400);
            nestedItem.SetHeight(300);
            doc.Add(list);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListSetSymbol() {
            List list = new List();
            //Assert.assertEquals("- ", ((Text) list.getProperty(Property.LIST_SYMBOL)).getText());
            NUnit.Framework.Assert.AreEqual(null, list.GetProperty<Object>(Property.LIST_SYMBOL));
            list.SetListSymbol("* ");
            NUnit.Framework.Assert.AreEqual("* ", ((Text)list.GetProperty<Object>(Property.LIST_SYMBOL)).GetText());
            list = new List();
            Style style = new Style();
            style.SetProperty(Property.LIST_SYMBOL, new Text("* "));
            list.AddStyle(style);
            NUnit.Framework.Assert.AreEqual("* ", ((Text)list.GetProperty<Object>(Property.LIST_SYMBOL)).GetText());
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ListItemNullSymbol() {
            String outFileName = destinationFolder + "listItemNullSymbol.pdf";
            String cmpFileName = sourceFolder + "cmp_listItemNullSymbol.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFileName));
            Document doc = new Document(pdfDoc);
            List list = new List();
            list.Add(new ListItem("List item 1"));
            Text listSymbolText = null;
            ListItem listItem2 = new ListItem("List item 2").SetListSymbol(listSymbolText);
            list.Add(listItem2);
            list.Add(new ListItem("List item 3"));
            doc.Add(list);
            doc.Add(new LineSeparator(new DashedLine()));
            list.SetListSymbol(ListNumberingType.ENGLISH_LOWER);
            doc.Add(list);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [NUnit.Framework.Ignore("DEVSIX-1001")]
        public virtual void ListSymbolForcedPlacement01() {
            String outFileName = destinationFolder + "listSymbolForcedPlacement01.pdf";
            String cmpFileName = sourceFolder + "cmp_listSymbolForcedPlacement01.pdf";
            PdfDocument pdf = new PdfDocument(new PdfWriter(outFileName));
            Document document = new Document(pdf);
            // This may seem like a contrived example, but in real life, this happened
            // with a two-column layout. The key is that the label is wider than the column.
            pdf.SetDefaultPageSize(PageSize.A7);
            document.Add(new Paragraph("Before list."));
            List l = new List();
            ListItem li = new ListItem().SetListSymbol("Aircraft of comparable role, configuration and era");
            l.Add(li);
            document.Add(l);
            document.Add(new Paragraph("After list."));
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff_"));
        }
    }
}
