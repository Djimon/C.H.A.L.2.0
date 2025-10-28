# .github/scripts/docgen.py

_Automatisch generiert/aktualisiert._

### Zweck der Datei
Automatisierte Generierung von Dokumentation für Quellcodedateien in einem Git-Repository.

### Öffentliche API
- **Funktionen:**
  - `changed_files_since_last_commit() -> List[str]`
    - **Rückgabewert:** Liste der geänderten Dateien seit dem letzten Commit mit unterstützten Dateiendungen.
  
  - `read_text(path: str) -> str`
    - **Parameter:** `path` - Pfad zur Datei.
    - **Rückgabewert:** Inhalt der Datei als String.
  
  - `write_if_changed(path: pathlib.Path, content: str) -> bool`
    - **Parameter:** 
      - `path` - Pfad zur Zieldatei.
      - `content` - Inhalt, der geschrieben werden soll.
    - **Rückgabewert:** `True`, wenn die Datei geändert wurde, sonst `False`.
  
  - `doc_path_for(src_path: str) -> pathlib.Path`
    - **Parameter:** `src_path` - Pfad zur Quellcodedatei.
    - **Rückgabewert:** Pfad zur Dokumentationsdatei.
  
  - `all_repo_files() -> List[str]`
    - **Rückgabewert:** Liste aller getrackten Dateien im Repository mit unterstützten Endungen.
  
  - `files_to_process() -> List[str]`
    - **Rückgabewert:** Liste der Dateien, die verarbeitet werden sollen (entweder alle oder nur geänderte).
  
  - `llm_markdown_for(path: str, code: str) -> str`
    - **Parameter:**
      - `path` - Pfad zur Quellcodedatei.
      - `code` - Quellcode als String.
    - **Rückgabewert:** Generierte Markdown-Dokumentation als String.
  
  - `main()`
    - **Rückgabewert:** Keine (führt die Hauptlogik aus).

### Wichtige Abläufe / Nebenwirkungen
- Liest geänderte Dateien oder alle Dateien (je nach Umgebungsvariable).
- Generiert Dokumentation mithilfe eines LLM (OpenAI).
- Schreibt die generierte Dokumentation in Markdown-Dateien im `docs`-Verzeichnis.
- Aktualisiert eine Indexdatei mit Links zu den generierten Dokumenten.

### Randbedingungen/Fehlerfälle
- Fehler beim Zugriff auf das Git-Repository oder beim Lesen von Dateien werden ignoriert.
- Bei fehlenden Änderungen wird eine entsprechende Nachricht ausgegeben.
- Umgebungsvariable `FULL_SCAN` steuert, ob alle Dateien oder nur geänderte Dateien verarbeitet werden.

### Kurzes Beispiel
```bash
# Um alle Dateien zu scannen
export FULL_SCAN=true
python .github/scripts/docgen.py
```
