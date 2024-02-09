# Image Resizer

Monitors a folder and subfolders and resizes or converts images based on the ancestor folder name.

Basically you set up a monitor folder using the app. While the app is running, it will look for directories like the following under that folder:

`[<min_width>][x<min_height>][_[<max_width>][x<max_height>]][.<format>]`

Examples (all preserve aspect ratio)

- _640 (max width of 640)

- _640.jpg (max width of 640, convert to jpg)

- 320_640 (min width of 320, max of 640)

- 320x200 (min size of 320x200)

- 320x200_640x480.png (min size of 320x200, max size of 640x480, convert to png)
- 