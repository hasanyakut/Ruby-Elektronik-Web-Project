// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    const loadMoreButton = document.getElementById('load-more');
    const productsGrid = document.getElementById('products-grid');

    if (loadMoreButton && productsGrid) {
        const total = parseInt(loadMoreButton.getAttribute('data-total') || '0', 10);
        let loaded = parseInt(loadMoreButton.getAttribute('data-loaded') || '0', 10);
        const pageSize = 4;

        if (loaded >= total) {
            loadMoreButton.style.display = 'none';
        }

        loadMoreButton.addEventListener('click', async function () {
            loadMoreButton.disabled = true;
            const originalText = loadMoreButton.innerHTML;
            loadMoreButton.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Yükleniyor...';

            try {
                const response = await fetch(`/Home/LoadMoreProducts?skip=${loaded}&take=${pageSize}`);
                if (!response.ok) throw new Error('İstek başarısız oldu');
                const html = await response.text();

                const temp = document.createElement('div');
                temp.innerHTML = html;
                const newCards = temp.children;
                while (newCards.length > 0) {
                    productsGrid.appendChild(newCards[0]);
                }

                loaded += pageSize;
                loadMoreButton.setAttribute('data-loaded', String(loaded));

                if (loaded >= total) {
                    loadMoreButton.style.display = 'none';
                }
            } catch (err) {
                console.error(err);
                loadMoreButton.innerHTML = 'Tekrar dene';
            } finally {
                loadMoreButton.disabled = false;
                if (loadMoreButton.style.display !== 'none') {
                    loadMoreButton.innerHTML = originalText;
                }
            }
        });
    }
});