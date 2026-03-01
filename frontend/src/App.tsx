import { useEffect, useState } from 'react';
import { getMessages, addMessage, deleteMessage } from './api/api';
import type { Message } from './types/message';

/**
 * The main application component that renders the message management interface.
 * @returns The JSX element representing the application UI.
 */
function App() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    loadMessages();
  }, []);

  /** Loads messages from the backend API. */
  async function loadMessages() {
    try {
      const data = await getMessages();
      setMessages(data);
    } finally {
      setLoading(false);
    }
  }

  /** Handles adding a new message. */
  async function handleAddMessage() {
    if (!newMessage.trim()) return;

    const created = await addMessage(newMessage);
    setMessages(prev => [created, ...prev]);
    setNewMessage('');
  }

  /** Handles deleting a message by its ID. */
  async function handleDeleteMessage(id: number) {
    await deleteMessage(id);
    setMessages(prev => prev.filter(m => m.id !== id));
  }

  return (
    <div className="min-h-screen bg-gray-50 flex justify-center">
      <div className="w-full max-w-3xl p-6">
        <h1 className="text-3xl font-bold mb-6 text-center">
          Docker Full-Stack Demo
        </h1>

        {/* Add Message */}
        <div className="flex gap-2 mb-6">
          <input
            type="text"
            className="flex-1 rounded-md bg-gray-200 px-3 py-1.5 text-base text-gray-900 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
            placeholder="Enter a message..."
            value={newMessage}
            onChange={e => setNewMessage(e.target.value)}
            onKeyDown={e => e.key === "Enter" && handleAddMessage()}
          />
          <button
            onClick={handleAddMessage}
            className="rounded-md bg-indigo-100 px-2 py-1 text-sm font-semibold text-indigo-600 shadow-xs hover:bg-indigo-200 cursor-pointer"
          >
            Add Message
          </button>
        </div>

        {loading && (
          <div className="p-4 text-gray-500">Loading messages...</div>
        )}

        {!loading && messages.length === 0 && (
          <div className="p-4 text-gray-500">No messages yet.</div>
        )}

        {!loading && messages.length > 0 && (
          <div className="mt-8 flow-root">
            <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
              <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                <table className="relative min-w-full divide-y divide-gray-300">
                  <thead>
                    <tr>
                      <th scope="col" className="py-3.5 pr-3 pl-4 text-left text-sm font-semibold text-gray-900 sm:pl-0">Message</th>
                      <th scope="col" className="py-3.5 pr-4 pl-3 sm:pr-0">
                        <span className="sr-only">Delete</span>
                      </th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200">
                    {messages.map(message => (
                      <tr key={message.id}>
                        <td className="whitespace-nowrap py-4 pr-3 pl-4 text-sm font-medium text-gray-900 sm:pl-0">{message.text}</td>
                        <td className="whitespace-nowrap py-4 pr-4 pl-3 text-right text-sm font-medium sm:pr-0">
                          <button
                            onClick={() => handleDeleteMessage(message.id)}
                            className="text-red-600 hover:text-red-800 cursor-pointer"
                          >
                            Delete
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        )}

      </div>
    </div>
  );
}

export default App;
