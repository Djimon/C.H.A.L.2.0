# BayatGames.SaveGameFree.Examples.ExampleSaveWeb

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Web/ExampleSaveWeb.cs`._

1) Purpose
- Unity MonoBehaviour example demonstrating save/load via SaveGameWeb over HTTP.
- Stores credentials, endpoint, and save identifier; uses active serializer for serialization.
- Updates a target Transform from keyboard input and persists/restores its position through web requests.

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleSaveWeb : MonoBehaviour
    - Public fields
      - public Transform target
        - The transform whose position is read/written during save/load.
      - public bool loadOnStart
      - public string identifier
        - Save key used for download/upload.
      - public string username
      - public string password
      - public string url
      - public bool encode
      - public string encodePassword
    - Public methods
      - public void Load()
        - Starts coroutine LoadEnumerator.
      - public void Save()
        - Starts coroutine SaveEnumerator.

3) Key Behavior & Side Effects
- Start
  - Invokes Load() unconditionally when the component starts.
- Update
  - Reads Input.GetAxis("Horizontal") and Input.GetAxis("Vertical") each frame.
  - Modifies target.position accordingly (positions x and y components).
- Load()
  - Starts coroutine LoadEnumerator().
- LoadEnumerator()
  - Logs "Downloading...".
  - Creates SaveGameWeb with credentials, url, encode options, and active serializer.
  - Yields on web.Download(identifier) to perform asynchronous download.
  - Sets target.position to the loaded Vector3Save value from web.Load<Vector3Save>(identifier, Vector3.zero).
  - Logs "Download Done.".
- Save()
  - Starts coroutine SaveEnumerator().
- SaveEnumerator()
  - Logs "Uploading...".
  - Creates SaveGameWeb with credentials, url, encode options, and active serializer.
  - Yields on web.Save<Vector3Save>(identifier, target.position) to perform asynchronous save.
  - Logs "Upload Done.".

4) Constraints & Failure Modes
- target must be assigned; otherwise NullReferenceException when accessing target.position.
- loadOnStart exists but is not referenced in code (Start() always calls Load()).
- No explicit error handling for web failures; behavior depends on SaveGameWeb implementation.
- Uses Unity coroutines; game object must remain active while operations run.
- Assumes Vector3Save is a serializable type compatible with web.Load<Vector3Save> and web.Save<Vector3Save>.

5) Example
- Not provided as a separate runnable snippet (the file itself serves as a runtime example).

6) Unknowns
- Exact behavior and error reporting of SaveGameWeb.Download/Save beyond the yielded coroutines.
- The specifics of Vector3Save serialization and how it maps to Vector3 (beyond default Vector3.zero as fallback).
- How loadOnStart is intended to control loading (not used by this file).
