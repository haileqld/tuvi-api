#!/usr/bin/env bash
set -euo pipefail

log() { echo "==> $*"; }

log "Checking for existing 'gemini'..."
if command -v gemini >/dev/null 2>&1; then
  log "gemini already installed at $(command -v gemini)"
  gemini --version || true
  exit 0
fi

log "Attempting installers: npm -> pip3"

if command -v npm >/dev/null 2>&1; then
  log "Trying npm install -g @google/gemini-cli"
  if npm install -g @google/gemini-cli >/tmp/gemini-npm.log 2>&1; then
    log "Installed @google/gemini-cli via npm"
    exit 0
  fi
  log "Trying npm install -g gemini-cli"
  if npm install -g gemini-cli >/tmp/gemini-npm2.log 2>&1; then
    log "Installed gemini-cli via npm"
    exit 0
  fi
fi

if command -v pip3 >/dev/null 2>&1; then
  log "Trying pip3 install --user gemini-cli"
  if pip3 install --user gemini-cli >/tmp/gemini-pip.log 2>&1; then
    log "Installed gemini-cli via pip3"
    export PATH="$HOME/.local/bin:$PATH"
    exit 0
  fi
  log "Trying pip3 install --user google-gemini"
  if pip3 install --user google-gemini >/tmp/gemini-pip2.log 2>&1; then
    log "Installed google-gemini via pip3"
    export PATH="$HOME/.local/bin:$PATH"
    exit 0
  fi
fi

log "Automatic install attempts failed. Check logs: /tmp/gemini-*.log"
exit 0
