import os
import zipfile

def compress_zip(zip_file,files):
    with zipfile.ZipFile(zip_file,'w',zipfile.ZIP_DEFLATED) as zipf:
        try:
            for file_path in files:
                print(file_path)
                relative_path = os.path.basename(file_path)
                zipf.write(file_path,arcname=relative_path)
        except:
            return "CANCELLED"

def compress_zip_dir(zip_file,dir_path):
    with zipfile.ZipFile(zip_file,'a',zipfile.ZIP_DEFLATED) as zipf:
        for folder, _, files in os.walk(dir_path):
            for file in files:
                absolute_path = os.path.join(folder, file)
                relative_path = os.path.relpath(absolute_path, os.path.dirname(dir_path))
                zipf.write(absolute_path, arcname=relative_path)

package_files = {
    "CHANGELOG.md",
    "icon.png",
    "manifest.json",
    "README.md",
    "LICENSE",
    "bin\\Debug\\netstandard2.1\\DarthLilo.LevelMusicLib.dll"
}

author = "DarthLilo"
modname = "LevelMusicLib"

version_output = input("Enter the version of the package: ")
zip_file = f"builds/{author}-{modname}-{version_output}.zip"
compress_zip(zip_file,package_files)
#compress_zip_dir(zip_file,"bin\\Debug\\netstandard2.1")
print("Finished building Thunderstore Package")