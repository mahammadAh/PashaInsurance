# String Filtering API

Simple  API for filtering large text files with chunked upload support.

##  Quick Start

```bash
cd Presentation
dotnet run
```

##  API Endpoints

### POST `/api/upload`
Uploads a text chunk for processing.

**Request Body:**
```json
{
  "uploadId": "unique-session-id",
  "chunkIndex": 0,
  "data": "Hello World",
  "isLastChunk": false
}
```

**Response:**
- `"Waiting for next chunk"` - if this is not the last chunk
- `"Accepted"` - if this is the last chunk and text is sent for filtering

### GET `/api/{uploadId}`
Gets the filtered result.

**Response:**
- Filtered text (if ready)
- `null` (if not found or still processing)

##  Filtering Configuration

In `appsettings.json`:

```json
{
  "Filtering": {
    "SimilarityThreshold": 0.8,
    "Strategy": "Levenshtein",
    "StopWords": ["spam", "bad", "virus", "foo", "bar"]
  }
}
```

### Available Algorithms:
- **`"Jaro-Winkler"`** - for precise word similarity detection
- **`"Levenshtein"`** - for Levenshtein distance-based filtering

### Parameters:
- **`SimilarityThreshold`** (0.0-1.0) - similarity threshold for word removal
- **`StopWords`** - array of stop words for filtering

##  How It Works

1. **Upload:** Client sends text in chunks via POST `/api/upload`
2. **Buffering:** System assembles chunks into complete text
3. **Queue:** When last chunk is received, text goes to processing queue
4. **Filtering:** Background Service processes texts asynchronously
5. **Result:** Client gets filtered text via GET `/api/{uploadId}`

##  Testing

```bash
dotnet test
```

Runs unit tests for filtering algorithms.

##  Architecture

```
Presentation (API Controllers)
    ↓
Application (Services + Background Processing)
    ↓
Domain (Models + Options)
```

- **Clean Architecture** with layer separation
- **Background Service** for asynchronous processing
- **In-memory** storage for simplicity



*Simple and efficient API for filtering large text files*
