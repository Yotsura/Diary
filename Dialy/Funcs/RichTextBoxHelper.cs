using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

//https://qiita.com/soi/items/ac970626af21aa5a362f 方法2 添付プロパティでFlowDocumentまるごとBindingする

namespace Dialy
{
    public class RichTextBoxHelper : DependencyObject
    {
        public static FlowDocument GetDocument(DependencyObject obj) => (FlowDocument)obj.GetValue(DocumentProperty);
        public static void SetDocument(DependencyObject obj, FlowDocument value) => obj.SetValue(DocumentProperty, value);

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.RegisterAttached(
            "Document", typeof(FlowDocument), typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata(null, Document_Changed));

        private static void Document_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is RichTextBox richTextBox))
                return;

            var attachedDocument = GetDocument(richTextBox);

            //FlowDocumentは1つのRichTextBoxにしか設定できない。
            //すでに他のRichTextBoxに所属しているなら、コピーを作成・設定する
            richTextBox.Document = attachedDocument.Parent == null
                ? attachedDocument
                : CopyFlowDocument(attachedDocument);
        }

        private static FlowDocument CopyFlowDocument(FlowDocument sourceDoc)
        {
            //もとのFlowDocumentをMemoryStream上に一度Serializeする
            var copyDoc = new FlowDocument();
            var sourceRange = new TextRange(sourceDoc.ContentStart, sourceDoc.ContentEnd);
            using (var stream = new MemoryStream())
            {
                XamlWriter.Save(sourceRange, stream);
                sourceRange.Save(stream, DataFormats.XamlPackage);

                //新しくFlowDocumentを作成
                var copyRange = new TextRange(copyDoc.ContentStart, copyDoc.ContentEnd);
                //MemoryStreamからDesirializeして書き込む
                copyRange.Load(stream, DataFormats.XamlPackage);
            }
            return copyDoc;
        }

        public static FlowDocument CreateFlowDoc(string innerText)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(innerText));
            var result = new FlowDocument(paragraph) { PageWidth = 2000 };
            return result;
        }

        public static FlowDocument CreateFlowDoc(List<Run> runs)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.AddRange(runs);
            var result = new FlowDocument(paragraph) { PageWidth = 2000 };
            return result;
        }
    }
}
