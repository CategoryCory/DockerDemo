import { useEffect, useState } from 'react';
import { getMessages, addMessage, deleteMessage } from './api/api';
import type { Message } from './types/message';

function App() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    loadMessages();
  }, []);

  async function loadMessages() {
    try {
      const data = await getMessages();
      setMessages(data);
    } finally {
      setLoading(false);
    }
  }

  async function handleAddMessage() {
    if (!newMessage.trim()) return;

    const created = await addMessage(newMessage);
    setMessages(prev => [created, ...prev]);
    setNewMessage('');
  }

  async function handleDeleteMessage(id: number) {
    await deleteMessage(id);
    setMessages(prev => prev.filter(m => m.id !== id));
  }

  return (
    <div className="min-h-screen bg-gray-100 flex justify-center">
      <div className="w-full max-w-xl p-6">
        <h1 className="text-3xl font-bold mb-6 text-center">
          Docker Full-Stack Demo
        </h1>

        {/* Add Message */}
        <div className="flex gap-2 mb-6">
          <input
            type="text"
            className="flex-1 rounded border px-3 py-2"
            placeholder="Enter a message..."
            value={newMessage}
            onChange={e => setNewMessage(e.target.value)}
            onKeyDown={e => e.key === "Enter" && handleAddMessage()}
          />
          <button
            onClick={handleAddMessage}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            Add Message
          </button>
        </div>

        {/* Message List */}
        <div className="bg-white rounded shadow">
          {loading && (
            <div className="p-4 text-gray-500">Loading messages...</div>
          )}

          {!loading && messages.length === 0 && (
            <div className="p-4 text-gray-500">No messages yet.</div>
          )}

          <ul>
            {messages.map(message => (
              <li
                key={message.id}
                className="flex items-center justify-between px-4 py-2 border-b last:border-b-0"
              >
                <span>{message.text}</span>
                <button
                  onClick={() => handleDeleteMessage(message.id)}
                  className="text-red-600 hover:text-red-800"
                >
                  Delete
                </button>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default App;
