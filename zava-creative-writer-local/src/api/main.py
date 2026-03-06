from pathlib import Path
from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from fastapi.middleware.cors import CORSMiddleware

from orchestrator import Task, create

base = Path(__file__).resolve().parent

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    # Local-only workshop demo — allow any origin for localhost testing
    allow_origins=["*"],
    allow_methods=["*"],
    allow_headers=["*"],
)


@app.get("/")
async def root():
    return {"message": "Zava Creative Writer — powered by Foundry Local"}


@app.post("/api/article")
async def create_article(task: Task):
    return StreamingResponse(
        create(task.research, task.products, task.assignment),
        media_type="text/event-stream",
    )
