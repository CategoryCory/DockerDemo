import type { Message } from "../types/message";

const API_BASE = '/api/message';

export async function getMessages(): Promise<Message[]> {
    const response = await fetch(API_BASE);

    if (!response.ok) throw new Error(`Failed to fetch messages: ${response.statusText}`);
    return response.json();
}

export async function addMessage(text: string): Promise<Message> {
    const response = await fetch(API_BASE, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ text })
    });

    if (!response.ok) throw new Error(`Failed to add message: ${response.statusText}`);
    return response.json();
}

export async function deleteMessage(id: number): Promise<void> {
    const response = await fetch(`${API_BASE}/${id}`, {
        method: 'DELETE'
    });

    if (!response.ok) throw new Error(`Failed to delete message: ${response.statusText}`);
}
