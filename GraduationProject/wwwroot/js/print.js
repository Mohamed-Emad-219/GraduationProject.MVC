function printContainer() {
    var printContents = document.getElementById("searchResults");
    document.getElementById("report").className = "";
    const clonedContent = printContents.cloneNode(true);
    const content = clonedContent.outerHTML;

    const printWindow = window.open('', '', 'height=800,width=1000');
    let headContent = document.querySelector("head").innerHTML;
    printWindow.document.write(`
        <html>
            <head>${headContent}
                <title>Report</title>
                <style>
                    @@page { margin: 15mm; }
                    @@media print { * { -webkit-print-color-adjust: exact; } }
                    html, body { 
                        margin: 0 !important; 
                        padding: 0 !important; 
                        background-color: #fff !important; 
                    }
                    .btn { display: none !important; } /* Hide buttons */
                </style>
            </head>
            <body>${content}</body>
        </html>`);
    printWindow.document.close();
    printWindow.focus();
    printWindow.onload = function () {
        printWindow.print();
    };
}

