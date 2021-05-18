import PDFObject from "pdfobject";

export default () => {
    $('.jsPdfPreview').each((i, e) => {
        let url = $(e).attr('mediaUrl');
        let height = $(e).attr('height');
        PDFObject.embed(url, e, { height: height + "px" });
    })
}