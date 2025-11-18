import pathlib

root = pathlib.Path(".")

bad_files = []

for path in root.rglob("*.cs"):
    if ".git" in path.parts:
        continue
    # externe Sachen ggf. skippen:
    if "xTernal" in path.parts or "ThirdParty" in path.parts:
        continue

    try:
        path.read_text(encoding="utf-8")
    except UnicodeDecodeError:
        bad_files.append(path)

print("Nicht-UTF-8 Dateien:")
for p in bad_files:
    print(" -", p)
