---

system_name: Save-API One-Pager
namespace: BayatGames.SaveGameFree
main_class: SaveGame (static)
defaults:
serializer: SaveGameJsonSerializer
encoding: UTF8
encode: false
save_path: PersistentDataPath
encode_password: "<SET_AT_RUNTIME>"
encryption:
enabled_flag: Encode
algorithm: AES-256-CBC
pipeline: JSON -> Base64 -> SaveGameSimpleEncoder
io_fallback:
no_filesystem: PlayerPrefs
check: IOSupported()
events:

* OnSaved
* OnLoaded
  callbacks:
* SaveCallback
* LoadCallback
  helpers:
* ColorSave (implicit <-> UnityEngine.Color)
* Vector3Save (implicit <-> Vector2/3/4 & *Save)
* MeshSave (implicit <-> UnityEngine.Mesh)
  platform_notes:
* UWP: BinarySerializer unsupported
* Exists() may return true for directories
* DeleteAll() clears entire root path
  quick_recipes:
* standard_json_unencrypted
* encrypted_aes256
* custom_binary_serializer
* unity_types_helpers
* list_delete_files
  version: v1.1 (adds YAML front summary)

---

# Save-API – One‑Pager

> **Ziel:** Maximale Informationsdichte in 1 Datei, damit Menschen & LLMs das Save‑System ohne Kontext sofort korrekt nutzen. Enthält Signaturen, Defaults, Rezepte, Fallstricke.

---

## TL;DR (Digest für LLMs)

* **Namespace:** `BayatGames.SaveGameFree`
* **Zentrale Klasse:** `SaveGame` (static)
* **Defaults:** JSON (`SaveGameJsonSerializer`), UTF‑8, `Encode=false`, `SavePath=PersistentDataPath`, `EncodePassword` **selbst setzen!**
* **Verschlüsselung:** `Encode=true` ⇒ Base64(JSON) → `SaveGameSimpleEncoder` (AES‑256 CBC)
* **IO‑Fallback:** Kein Dateisystem ⇒ `PlayerPrefs`
* **Events:** `OnSaved`, `OnLoaded` (+ optionale `SaveCallback`, `LoadCallback`)
* **Helpers:** `ColorSave`, `Vector3Save`, `MeshSave` (implizite Casts ↔ Unity‑Typen)

---

## Public Surface (Signaturen)

### `SaveGame`

```csharp
// Konfiguration (static)
ISaveGameSerializer SaveGame.Serializer { get; set; }         // Default: SaveGameJsonSerializer
ISaveGameEncoder   SaveGame.Encoder   { get; set; }           // Default: SaveGameSimpleEncoder
Encoding           SaveGame.DefaultEncoding { get; set; }     // Default: UTF8
bool               SaveGame.Encode { get; set; }              // Default: false
SaveGamePath       SaveGame.SavePath { get; set; }            // Default: PersistentDataPath
string             SaveGame.EncodePassword { get; set; }      // Default: "h@e#ll$o%^" (ersetzen!)
bool               SaveGame.LogError { get; set; }            // Default: false

// Events/Callbacks
event SaveHandler SaveGame.OnSaved;
event LoadHandler SaveGame.OnLoaded;
SaveHandler SaveGame.SaveCallback;     // optional einmalig\ nLoadHandler SaveGame.LoadCallback;     // optional einmalig

// Save – Kurzformen (rufen Vollform)
void Save<T>(string id, T obj);
void Save<T>(string id, T obj, bool encode);
void Save<T>(string id, T obj, string password);
void Save<T>(string id, T obj, ISaveGameSerializer serializer);
void Save<T>(string id, T obj, ISaveGameEncoder encoder);
void Save<T>(string id, T obj, Encoding encoding);
void Save<T>(string id, T obj, SaveGamePath path);

// Save – Vollform (Kern)
void Save<T>(string id, T obj, bool encode, string password,
             ISaveGameSerializer serializer, ISaveGameEncoder encoder,
             Encoding encoding, SaveGamePath path);

// Load – Kurzformen
T Load<T>(string id);
T Load<T>(string id, T defaultValue);
T Load<T>(string id, bool encode, string password);
T Load<T>(string id, ISaveGameSerializer serializer);
T Load<T>(string id, ISaveGameEncoder encoder);
T Load<T>(string id, Encoding encoding);
T Load<T>(string id, SaveGamePath path);

// Load – Varianten mit defaultValue/encode/password/... existieren entsprechend

// Load – Vollform (Kern)
T Load<T>(string id, T defaultValue, bool encode, string password,
          ISaveGameSerializer serializer, ISaveGameEncoder encoder,
          Encoding encoding, SaveGamePath path);

// FS‑Utils
bool Exists(string id);
bool Exists(string id, SaveGamePath path);
void Delete(string id);
void Delete(string id, SaveGamePath path);
void Clear();
void Clear(SaveGamePath path);
void DeleteAll();
void DeleteAll(SaveGamePath path);
FileInfo[] GetFiles();
FileInfo[] GetFiles(string id);
FileInfo[] GetFiles(string id, SaveGamePath path);
DirectoryInfo[] GetDirectories();
DirectoryInfo[] GetDirectories(string id);
DirectoryInfo[] GetDirectories(string id, SaveGamePath path);
bool IOSupported();
bool IsFilePath(string str);
```

### Encoder / Serializer

```csharp
// Encoder
string SaveGameSimpleEncoder.Encode(string input, string password);
string SaveGameSimpleEncoder.Decode(string input, string password);

// Serializer
void SaveGameJsonSerializer.Serialize<T>(T obj, Stream s, Encoding enc);
T    SaveGameJsonSerializer.Deserialize<T>(Stream s, Encoding enc);

void SaveGameBinarySerializer.Serialize<T>(T obj, Stream s, Encoding enc);
T    SaveGameBinarySerializer.Deserialize<T>(Stream s, Encoding enc);
```

### Datentyp‑Helper (implizite Casts)

```csharp
// Color
struct ColorSave { float r,g,b,a; }
implicit ColorSave  ←→ UnityEngine.Color

// Vektoren
struct Vector3Save { float x,y,z; }
implicit Vector2/3/4 ←→ Vector2Save/3Save/4Save

// Mesh
class MeshSave {
  Vector3Save[] vertices; int[] triangles; Vector2Save[] uv;
  Vector3Save[] normals; Color[] colors; Color32[] colors32;
}
implicit MeshSave  ←→ UnityEngine.Mesh
```

---

## Verhalten & Plattformen

* **Pfadlogik:** `id` absolut ⇒ unverändert. Sonst: `<SavePath>/<id>`
* **Encode=true:** JSON→Memory→Base64→Encoder(**AES‑256**, CBC, PKCS7, Salt+IV)→**Text** speichern
* **Encode=false:** rohe Serializer‑Bytes (Datei) bzw. `PlayerPrefs` (ohne IO)
* **Kein IO (z. B. WebGL):** `PlayerPrefs` statt Datei; `IOSupported()` prüfen
* **UWP‑Hinweis:** BinarySerializer nicht unterstützt
* **Exists():** meldet *Datei oder Ordner* als „vorhanden“; für reine Datei‑Checks zusätzlich über `FileInfo`
* **DeleteAll():** leert kompletten Root‑Pfad (Gefahr in `DataPath`)

---

## Quick Start (rezepthaft)

**1) Standard (JSON, unverschlüsselt)**

```csharp
SaveGame.Encode = false; // Default
SaveGame.Save("profile/player.json", playerState);
var state = SaveGame.Load<PlayerState>("profile/player.json");
```

**2) Verschlüsselt (AES‑256 via SimpleEncoder)**

```csharp
SaveGame.Encode = true;
SaveGame.EncodePassword = "CHANGE_ME";
SaveGame.Save("saves/slot1.dat", gameState);
var state = SaveGame.Load<GameState>("saves/slot1.dat");
```

**3) Custom Serializer (Binary – nicht UWP)**

```csharp
var bin = new SaveGameBinarySerializer();
SaveGame.Save("bin/slot1.bin", gameState, bin);
var state = SaveGame.Load<GameState>("bin/slot1.bin", bin);
```

**4) Unity‑Typen sicher speichern**

```csharp
MeshSave ms = someMesh;        // impliziter Cast
SaveGame.Save("meshes/m1.json", ms);
Mesh meshBack = SaveGame.Load<MeshSave>("meshes/m1.json");
```

**5) Dateien prüfen/listen/löschen**

```csharp
if (SaveGame.Exists("saves/slot1.dat")) { /* ... */ }
foreach (var fi in SaveGame.GetFiles("saves")) Debug.Log(fi.Name);
SaveGame.Delete("saves/slot1.dat");
```

---

## Events & Debug Hooks

```csharp
SaveGame.OnSaved  += (obj, id, enc, pwd, ser, encod, encg, path) => Debug.Log($"Saved {id}");
SaveGame.OnLoaded += (obj, id, enc, pwd, ser, encod, encg, path) => Debug.Log($"Loaded {id}");

// Optional einmalige Callbacks (setzen, aufrufen lassen, dann selbst zurücksetzen)
SaveGame.SaveCallback = /* ... */;
SaveGame.LoadCallback = /* ... */;
```

> **Editor‑Debug:** Du kannst in Utility‑MonoBehaviours `[ContextMenu]`‑Methoden anlegen (z. B. `SaveNow()`, `LoadNow()`), die die obigen Rezepte aufrufen.

---

## Best Practices (kurz & hart)

* **Passwort immer setzen** (nicht build‑fixe Defaults!)
* **Einheitliche IDs** (z. B. `saves/slotX.dat`), nie gemischt Ordner/Datei unter gleicher ID
* **Versionierung** im Root‑Objekt (`SaveVersion`) für spätere Migrationen
* **Atomic Saves:** zuerst temp schreiben, dann austauschen (Datei‑Korrumpierung vermeiden)
* **Größenlimits** bei `PlayerPrefs` im Auge behalten (WebGL)

---

## FAQ (kurz)

* **Wie wähle ich einen anderen Speicherort?** `SaveGame.SavePath = SaveGamePath.DataPath` *oder* absoluten Pfad als `id` übergeben.
* **Kann ich beides – verschlüsselt & binär?** Ja: `Encode=true` + `SaveGameBinarySerializer` (außer UWP).
* **Wie finde ich alle Saveslots?** `GetFiles("saves")` und Dateinamen parsen.
* **Warum lädt `Load<T>` `default(T)`?** Datei nicht vorhanden oder Deserialization‑Fehler → `defaultValue` wird zurückgegeben.

---

