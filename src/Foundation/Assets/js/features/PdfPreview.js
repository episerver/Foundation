class PdfPreview {
    Render() {
        $('.jsPdfPreview').each(function (i, e) {
            let url = $(e).attr('mediaUrl');
            let height = $(e).attr('height');
            PDFObject.embed(url, e, { height: height + "px" });
        })
    }
}