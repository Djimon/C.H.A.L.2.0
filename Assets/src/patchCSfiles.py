import pathlib

root = pathlib.Path(".")

for path in root.rglob("*.cs"):
    # Drittanbieter ggf. auslassen
    if "xTernal" in path.parts or "ThirdParty" in path.parts:
        continue

    # Versuchen, als cp1252 zu lesen
    try:
        raw = path.read_bytes()
        text = raw.decode("cp1252")
    except UnicodeDecodeError:
        print(f"Skip (not cp1252?): {path}")
        continue

    # Wieder als UTF-8 speichern
    path.write_text(text, encoding="utf-8", newline="\n")
    print(f"Converted to UTF-8: {path}")
