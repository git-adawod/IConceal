# IConceal #
A mini desktop application designed to using WPF, C# for watermarking and hiding data. It's a project that piqued my interest
when I researched of different methods of steganography and although this isn't as sophisticated as it can be, it was a very good
learning opportunity.

## Features ##
- [X] Hiding Files and Directories within an Image
- [X] Hiding Text within an Image's pixels
- [X] Hiding Text within Text
- [ ] Hiding Data within an Audio File

## Image Steganography ##
There are two types implemented :
* Hiding text inside the pixels of an image
* Hiding zipped files/directories within the binary data of the image

### Color Steganography ###
For the color algorithm, a letter, say **'M'** is converted into binary -> **01001101** and padded with enough zeros to make it have
a total of 9 digits -> **001001101**. This is then split into 3 groups **001 001 101** and each group is put into the LSBs of the
pixel's R, G and B values that are also padded with enough zeros to make them have 9 digits;  
say **RGB(66, 135, 245)** -> **(001000010, 010000111, 011110101)** so then the groups of the letter **'M'** are placed into the last
three significant bits -> **001 001 101** + **(001000010, 010000111, 011110101)** = **(001000001, 010000001, 011110101)** which is  
**RGB(65, 129, 245)** a new color with almost an indistinguishable difference. The reverse is done to retrieve the data.    there is metadata that is inserted as well within the image: a watermark 'IConceal' starting at the bottom left corner pixel, to ensure that the image can be decoded with this program; the length of the text starting at the top left corner pixel, each pixel onwards holding a digit, e.g. **length = 365 -> pixel(0,0) = 3, pixel(1, 0) = 6, pixel(2, 0) = 5** and the length of this, referred to as **'padding'** is stored in the top right corner pixel, so if length is **365**, then padding is **3**.
 
### Binary Embed Steganography ###
For the Binary Embed algorithm, a **'Carrier Image'** is chosen and file(s)/directory that want to be hidden are selected. The chosen file(s)/directory are compressed and archived to a temporary zip file created by the program. A new image 'Payload Image' is then created by copying chunks of it as well as the zip file into a this new Bitmap. To decode, the image can simply be dropped into a compressing software such as winRAR.

## Text Steganography ##
A payload text is typed out as well as a 'secret' text. The **'secret'** is converted into binary and it's 1's are converted to a **zero-width space (\u200B)**, the 0's are converted to a **zero-width non-joiner (\u200C)**. The letters are separated by a **zero-width joiner (\u200D)** and each word is distinguished by a **zero-width non-breaking space (\uFEFF)** and the zero-width string is returned and appended to the Payload text.

