function printContainer() {
    var printContents = document.getElementById("searchResults");
    document.getElementById("report").className = "";
    const clonedContent = printContents.cloneNode(true);

    const originalCanvases = printContents.querySelectorAll('canvas');
    const clonedCanvases = clonedContent.querySelectorAll('canvas');

    const promises = [];

    originalCanvases.forEach((canvas, index) => {
        const dataUrl = canvas.toDataURL("image/png");
        const img = document.createElement("img");
        img.src = dataUrl;
        img.style.width = canvas.style.width;
        img.style.height = canvas.style.height;

        // Wait until image is loaded
        const promise = new Promise((resolve) => {
            img.onload = () => {
                clonedCanvases[index].parentNode.replaceChild(img, clonedCanvases[index]);
                resolve();
            };
        });

        promises.push(promise);
    });

    Promise.all(promises).then(() => {
        const content = clonedContent.outerHTML;

        const printWindow = window.open('', '', 'height=800,width=1000');
        let headContent = document.querySelector("head").innerHTML;
        printWindow.document.write(`
            <html>
                <head>${headContent}
                    <title>Report</title>
                    <style>
                        @page {
                            margin: 15mm;
                            @bottom-right {
                                content: "Page " counter(page) " of " counter(pages);
                                font-size: 12px;
                                color: black;
                            }
                        }
                        @media print {
                            * { -webkit-print-color-adjust: exact; }
                        }
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
    });
}
