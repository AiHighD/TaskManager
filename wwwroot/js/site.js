function previewImage(event) {
    const input = event.target;
    const file = input.files[0];

    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = function(e) {
            const preview = document.getElementById('preview');
            preview.src = e.target.result;
            preview.classList.remove('d-none'); // Show image preview
        };
        reader.readAsDataURL(file);
    } else {
        alert('Please select an image file.');
    }
}

function previewFile(event) {
    const input = event.target;
    const file = input.files[0];

    if (file) {
        const fileNameElement = document.getElementById('file-name');
        const fileIconElement = document.getElementById('file-icon');
        const fileInfoElement = document.getElementById('file-info');
        const filePreviewElement = document.getElementById('file-preview');

        fileNameElement.textContent = `File Name: ${file.name}`; // Display the file name
        fileInfoElement.textContent = `File Size: ${(file.size / 1024).toFixed(2)} KB`; // Display file size

        // Show the file preview section
        filePreviewElement.classList.remove('d-none');

        // Handle file types and display the preview accordingly
        const iconMapping = {
            'application/pdf': 'pdf-icon.png',
            'application/msword': 'docx-icon.png',
            'application/vnd.openxmlformats-officedocument.wordprocessingml.document': 'docx-icon.png',
            'application/vnd.ms-excel': 'xlsx-icon.png',
            'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': 'xlsx-icon.png',
            'text/plain': 'txt-icon.png',
            'image/': 'image-icon.png'
        };

        let iconSrc = null;

        // Check if the file type has a corresponding icon
        if (file.type.startsWith('image/')) {
            iconSrc = '../assets/images/icons/image-icon.png';
        } else if (file.type === 'application/pdf') {
            iconSrc = '../assets/images/icons/pdf-icon.png';
        } else if (file.type === 'application/msword' || file.type === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document') {
            iconSrc = '../assets/images/icons/docx-icon.png';
        } else if (file.type === 'application/vnd.ms-excel' || file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
            iconSrc = '../assets/images/icons/xlsx-icon.png';
        } else if (file.type === 'text/plain') {
            iconSrc = '../assets/images/icons/txt-icon.png';
        } else {
            // If no icon found, show default icon or no icon
            iconSrc = '../assets/images/icons/default-icon.png';
        }

        // Update the icon element
        const iconImage = document.createElement('img');
        iconImage.src = iconSrc;
        iconImage.alt = 'File Icon';
        iconImage.classList.add('file-icon-image');
        iconImage.style.width = '80px'; // You can adjust the size as needed

        // Clear previous icon and add the new one
        fileIconElement.innerHTML = ''; // Clear any existing icon
        fileIconElement.appendChild(iconImage); // Append new icon
        fileIconElement.classList.remove('d-none'); // Show icon
    } else {
        alert('No file selected.');
    }
}
