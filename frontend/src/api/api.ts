import type { Message } from "../types/message";

const API_BASE = '/api/message';

/**
 * Fetches all messages from the backend API.
 * @returns A promise that resolves to an array of Message objects.
 * @throws An error if the request fails.
 */
export async function getMessages(): Promise<Message[]> {
    const response = await fetch(API_BASE);

    if (!response.ok) throw new Error(`Failed to fetch messages: ${response.statusText}`);
    return response.json();
}

/**
 * Adds a new message to the backend API.
 * @param text The text content of the message to be added.
 * @returns A promise that resolves to the created Message object.
 * @throws An error if the request fails.
 */
export async function addMessage(text: string): Promise<Message> {
    const response = await fetch(API_BASE, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ text })
    });

    if (!response.ok) throw new Error(`Failed to add message: ${response.statusText}`);
    return response.json();
}

/**
 * Deletes a message from the backend API by its ID.
 * @param id The unique identifier of the message to be deleted.
 * @returns A promise that resolves when the message is successfully deleted.
 * @throws An error if the request fails.
 */
export async function deleteMessage(id: number): Promise<void> {
    const response = await fetch(`${API_BASE}/${id}`, {
        method: 'DELETE'
    });

    if (!response.ok) throw new Error(`Failed to delete message: ${response.statusText}`);
}
