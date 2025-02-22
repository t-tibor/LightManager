using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LightManager.Domain.Devices.Motion;
using LightManager.Persistence;

namespace LightManager.Web.Controllers
{
    public class MqttMotionSensorConfigsController : Controller
    {
        private readonly LightManagerDbContext _context;

        public MqttMotionSensorConfigsController(LightManagerDbContext context)
        {
            _context = context;
        }

        // GET: MqttMotionSensorConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.MqttMotionSensorConfigs.ToListAsync());
        }

        // GET: MqttMotionSensorConfigs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mqttMotionSensorConfig = await _context.MqttMotionSensorConfigs
                .FirstOrDefaultAsync(m => m.Name == id);
            if (mqttMotionSensorConfig == null)
            {
                return NotFound();
            }

            return View(mqttMotionSensorConfig);
        }

        // GET: MqttMotionSensorConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MqttMotionSensorConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,MotionDetectorTopic")] MqttMotionSensorConfig mqttMotionSensorConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mqttMotionSensorConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mqttMotionSensorConfig);
        }

        // GET: MqttMotionSensorConfigs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mqttMotionSensorConfig = await _context.MqttMotionSensorConfigs.FindAsync(id);
            if (mqttMotionSensorConfig == null)
            {
                return NotFound();
            }
            return View(mqttMotionSensorConfig);
        }

        // POST: MqttMotionSensorConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,MotionDetectorTopic")] MqttMotionSensorConfig mqttMotionSensorConfig)
        {
            if (id != mqttMotionSensorConfig.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mqttMotionSensorConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MqttMotionSensorConfigExists(mqttMotionSensorConfig.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mqttMotionSensorConfig);
        }

        // GET: MqttMotionSensorConfigs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mqttMotionSensorConfig = await _context.MqttMotionSensorConfigs
                .FirstOrDefaultAsync(m => m.Name == id);
            if (mqttMotionSensorConfig == null)
            {
                return NotFound();
            }

            return View(mqttMotionSensorConfig);
        }

        // POST: MqttMotionSensorConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mqttMotionSensorConfig = await _context.MqttMotionSensorConfigs.FindAsync(id);
            if (mqttMotionSensorConfig != null)
            {
                _context.MqttMotionSensorConfigs.Remove(mqttMotionSensorConfig);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MqttMotionSensorConfigExists(string id)
        {
            return _context.MqttMotionSensorConfigs.Any(e => e.Name == id);
        }
    }
}
