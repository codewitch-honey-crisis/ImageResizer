using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageResizer
{
	public partial class Main : Form
	{

		public Main()
		{
			InitializeComponent();
		}
		struct _DirEntry
		{
			public _DirEntry(Size min,Size max, ImageFormat format)
			{
				Min = min; Max = max; Format = format;
			}
			public Size Min;
			public Size Max;
			public ImageFormat Format;
			public static readonly _DirEntry Empty = new _DirEntry(Size.Empty, Size.Empty, ImageFormat.MemoryBmp);
			public override int GetHashCode()
			{
				return Min.GetHashCode()^Max.GetHashCode()^Format.GetHashCode();
			}
			public bool IsEmpty
			{
				get { return Min.IsEmpty && Max.IsEmpty && Format == ImageFormat.MemoryBmp; }
			}
			
		}
		static Size _CrackSize(string str)
		{
			if(string.IsNullOrWhiteSpace(str))
			{
				return Size.Empty;
			}
			Size result = Size.Empty;
			int i = str.IndexOf("x", StringComparison.OrdinalIgnoreCase);
			if(i==-1 || i == str.Length-1)
			{
				if(!int.TryParse(str.Replace("x", ""), out i))
				{
					return Size.Empty;
				}
				result.Width = i;
				return result;
			}
			if(i==0)
			{
				
				if (!int.TryParse(str.Replace("x", ""), out i))
				{
					return Size.Empty;
				}
				result.Height = i;
				return result;
			}
			int tmp;
			if(!int.TryParse(str.Substring(0,i),out tmp))
			{
				return Size.Empty;
			}
			result.Width = i;
			if (!int.TryParse(str.Substring(i+1), out tmp))
			{
				return Size.Empty;
			}
			result.Height= tmp;
			return result;
		}
		static ImageFormat _CrackFormat(string fmt)
		{
			ImageFormat result;
			switch (fmt.ToLowerInvariant())
			{
				case "jpg":
				case "jpeg":
					result= ImageFormat.Jpeg; break;
				case "gif":
					result= ImageFormat.Gif; break;
				case "bmp":
					result= ImageFormat.Bmp; break;
				case "png":
					result= ImageFormat.Png; break;
				case "ico":
					result= ImageFormat.Icon; break;
				case "wmf":
					result= ImageFormat.Wmf; break;
				case "exif":
					result= ImageFormat.Exif; break;
				case "emf":
					result= ImageFormat.Emf; break;
				case "tiff":
				case "tif":
					result= ImageFormat.Tiff; break;
				default:
					result = ImageFormat.MemoryBmp; break; 
			}
			return result;
		}
		static string _FormatToStr(ImageFormat fmt)
		{
			if (fmt == ImageFormat.Jpeg)
			{
				return "jpg";
			} else if (fmt == ImageFormat.Gif)
			{
				return "gif";
			} else if(fmt == ImageFormat.Bmp)
			{
				return "bmp";
			} else if(fmt == ImageFormat.Png) 
			{
				return "png";
			} else if(fmt==ImageFormat.Icon)
			{
				return "ico";
			} else if(fmt == ImageFormat.Wmf)
			{
				return "wmf";
			} else if(fmt == ImageFormat.Exif)
			{
				return "exif";
			} else if (fmt == ImageFormat.Emf)
			{
				return "emf";
			} else if(fmt == ImageFormat.Tiff)
			{
				return "tiff";
			}
			return null;
		}
		static _DirEntry _CrackDir(string name)
		{
			_DirEntry result = _DirEntry.Empty;
			
			var i = name.IndexOf('.');
			var f = name.Substring(i + 1);
			name = name.Substring(0, i);
			result.Format = _CrackFormat(f);
			if(result.Format==ImageFormat.MemoryBmp)
			{
				return _DirEntry.Empty;
			}
			i = name.IndexOf('_');
			if(i==-1 || i==name.Length-1)
			{
				// min only
				result.Min = _CrackSize(name.Replace("_", ""));
				if(result.Min==Size.Empty)
				{
					return _DirEntry.Empty;
				}
			} else if(i==0)
			{
				// max only
				result.Max = _CrackSize(name.Replace("_", ""));
				if (result.Max == Size.Empty)
				{
					return _DirEntry.Empty;
				}
			} else
			{
				result.Min = _CrackSize(name.Substring(0, i));
				if (result.Min == Size.Empty)
				{
					return _DirEntry.Empty;
				}
				result.Max = _CrackSize(name.Substring(i+1));
				if (result.Max == Size.Empty)
				{
					return _DirEntry.Empty;
				}
			}
			return result;
		}
		private void Folder_Leave(object sender, EventArgs e)
		{
			if(!Directory.Exists(Folder.Text))
			{
				Folder.ForeColor = Color.Red;
				FolderWatcher.EnableRaisingEvents = false;
			} else
			{
				Folder.ForeColor = SystemColors.WindowText;
				FolderWatcher.Path = Folder.Text;
				FolderWatcher.EnableRaisingEvents = true;
			}
		}

		private void Folder_Enter(object sender, EventArgs e)
		{
			Folder.ForeColor = SystemColors.WindowText;
			FolderWatcher.EnableRaisingEvents = false;
		}

		private void Browse_Click(object sender, EventArgs e)
		{
			var result = FolderBrowser.ShowDialog(this);
			if(result == DialogResult.OK)
			{
				Folder.Text = FolderBrowser.SelectedPath;
				FolderWatcher.Path = Folder.Text;
				FolderWatcher.EnableRaisingEvents = true;
				Folder.ForeColor = SystemColors.WindowText;
			}
		}
		static _DirEntry _GetDirEntry(string path,bool isDir= false)
		{
			string p = (isDir)?path:Path.GetDirectoryName(path);
			while(!string.IsNullOrWhiteSpace(p))
			{
				var f = Path.GetFileName(p);
				var result = _CrackDir(f);
				if(!result.IsEmpty)
				{
					return result;
				}
				p = Path.GetDirectoryName(path);
			}
			return _DirEntry.Empty;
		}
		static void UpdateFile(FileSystemEventArgs e)
		{
			if (File.Exists(e.FullPath))
			{
				_DirEntry de = _GetDirEntry(e.FullPath, false);
				ImageFormat fmt = _CrackFormat(Path.GetExtension(e.FullPath).Substring(1));
				if (fmt != ImageFormat.MemoryBmp && !de.IsEmpty)
				{
					try
					{
						var save = false;
						Bitmap bmp2 = null;
						using (var bmp = Bitmap.FromFile(e.FullPath))
						{
							var sz = bmp.Size;
							double ratio = (double)sz.Width / (double)sz.Height;
							var newSZ = sz;
							if (!de.Max.IsEmpty)
							{
								if (de.Max.Width != 0 && de.Max.Width < sz.Width)
								{
									save = true;
									newSZ.Width = de.Max.Width;
									newSZ.Height = (int)(newSZ.Width / ratio);
									if (de.Max.Height != 0 && de.Max.Height < newSZ.Height)
									{
										newSZ.Height = de.Max.Height;
										newSZ.Width = (int)(newSZ.Width * ratio);
									}
								}
								else if (de.Max.Height != 0 && de.Max.Height < sz.Height)
								{
									save = true;
									newSZ.Height = de.Max.Height;
									newSZ.Width = (int)(newSZ.Height * ratio);
								}
							}
							if (!de.Min.IsEmpty)
							{
								if (de.Min.Width != 0 && de.Min.Width > sz.Width)
								{
									save = true;
									newSZ.Width = de.Min.Width;
									newSZ.Height = (int)(newSZ.Width / ratio);
									if (de.Min.Height != 0 && de.Min.Height > newSZ.Height)
									{
										newSZ.Height = de.Min.Height;
										newSZ.Width = (int)(newSZ.Width * ratio);
									}
								}
								else if (de.Min.Height != 0 && de.Min.Height > sz.Height)
								{
									save = true;
									newSZ.Height = de.Min.Height;
									newSZ.Width = (int)(newSZ.Height * ratio);
								}
							}
							if (de.Format == ImageFormat.MemoryBmp)
							{
								de.Format = fmt;
							}
							else
							{
								if (fmt != de.Format)
								{
									save = true;
								}
							}
							if (save)
							{
								bmp2 = new Bitmap(bmp, newSZ);
							}
						}
						if(save && bmp2 != null)
						{
							var fn = string.Concat(Path.Combine(Path.GetDirectoryName(e.FullPath), Path.GetFileNameWithoutExtension(e.FullPath)), ".", _FormatToStr(de.Format));
							try
							{
								bmp2.Save(fn, de.Format);
								bmp2.Dispose();
								bmp2 = null;
								if (Path.GetFullPath(fn) != Path.GetFullPath(e.FullPath))
								{
									File.Delete(e.FullPath);
								}
							}
							catch
							{
								if(bmp2!=null)
								{
									bmp2.Dispose();
									bmp2 = null;
								}
							}
						}
					}
					catch
					{

					}
				}
			}

		}
		private void FolderWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			UpdateFile(e);
		}

		private void FolderWatcher_Created(object sender, FileSystemEventArgs e)
		{
			UpdateFile(e);
		}

		private void FolderWatcher_Renamed(object sender, RenamedEventArgs e)
		{

		}
	}
}
