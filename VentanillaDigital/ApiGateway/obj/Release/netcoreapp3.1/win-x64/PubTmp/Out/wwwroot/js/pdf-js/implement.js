var scale = 1.0,
  pageNum = 1,
  totalPages = 0,
  pageRendering = false,
  pdfDoc = null,
  pageNumPending = null;

function previsualizacionMovil(dataFile) {
  // The workerSrc property shall be specified.
  pdfjsLib.GlobalWorkerOptions.workerSrc = "../build/pdf.worker.js";
  var pdfData = atob(dataFile);

  var loadingTask = pdfjsLib.getDocument({ data: pdfData });
  loadingTask.promise.then(
    function (pdf) {
      pdfDoc = pdf;
      totalPages = pdf.numPages;
      document.getElementById("page_count").textContent = totalPages;
      agregarFuncionesAElementos();
      renderPage(pdf, pageNum);
    },
    function (reason) {
      // PDF loading error
      console.error(reason);
    }
  );

  function agregarFuncionesAElementos() {
    var buttonPrev = document.getElementById("prev");
    buttonPrev.addEventListener("click", onPrevPage);

    var buttonNext = document.getElementById("next");
    buttonNext.addEventListener("click", onNextPage);
  }
}

function renderPage(pdf, pageNumber) {
  pageRendering = true;
  pdf.getPage(pageNumber).then(function (page) {
    var viewport = page.getViewport({ scale: scale });

    // Prepare canvas using PDF page dimensions
    var canvas = document.getElementById("the-canvas");
    var context = canvas.getContext("2d");
    canvas.height = viewport.height;
    canvas.width = viewport.width;

    // Render PDF page into canvas context
    var renderContext = {
      canvasContext: context,
      viewport: viewport,
    };
    var renderTask = page.render(renderContext);
    renderTask.promise.then(function () {
      pageRendering = false;
      if (pageNumPending !== null) {
        // New page rendering is pending
        renderPage(pdf, pageNumPending);
        pageNumPending = null;
      }
    });
    // Update page counters
    var content = document.getElementById("page_num");
    if (content != null) {
      content.textContent = pageNumber;
    }
  });
}
/**
 * If another page rendering in progress, waits until the rendering is
 * finised. Otherwise, executes rendering immediately.
 */
function queueRenderPage(num) {
  if (pageRendering) {
    pageNumPending = num;
  } else {
    renderPage(pdfDoc, num);
  }
}

/**
 * Displays previous page.
 */
function onPrevPage() {
  if (pageNum <= 1) {
    return;
  }
  pageNum--;
  queueRenderPage(pageNum);
}

/**
 * Displays next page.
 */
function onNextPage() {
  if (pageNum >= totalPages) {
    return;
  }
  pageNum++;
  queueRenderPage(pageNum);
}
