class PdfPreview {
    Render() {
        $('.jsPdfPreview').each(function (i, e) {
            let url = $(e).attr('mediaUrl');
            let height = parseInt($(e).attr('height'));
            if (!height || height <= 0) {
                height = 800;
            }
            PDFObject.embed(url, e, { height: height + "px" });
        })
    }
}