# Multiscreen Wallpaper Shifter

A tool that takes in a wallpaper, and generates a new image that when set as desktop
background should display properly across multiple monitors.

Under Windows with AMD drivers, running multiple monitors, it is possible to have one
image stretch across all displays by using the "Tile" option for the wallpaper.

Unfortunately, it seems that Windows also always puts the top left pixel at the top
left position of the primary display, regardless of its physical location. The solution
to this is to take the image you want, and cut and move sections and order them in the
same order. This is tedious and seemed like it could be automated so I wrote this console
application.

## Usage

At it's simplest, open the Windows command prompt and run `msws.exe image`, where `image`
is the wallpaper you want to modify. The modified file will be created in the same directory
as the .exe as `output.png`

### Options

`/o output`
Supply an output file or path instead of using the default.

`/f format`
Where `format` is one of `jpeg` or `png`. This sets the image format of the output file.

**Note:** The file extension of the file is not automatically appended. If using the format
option make sure the extension of the output file is consistent.

## Notes

- Multiscreen Wallpaper Shifter expects an image with horizontal resolution equal to
the sum of the horizontal resolutions of your displays.
- The program assumes that the monitors are arranged horizontally. Setups with vast
differences in vertical position will probably act strangely.